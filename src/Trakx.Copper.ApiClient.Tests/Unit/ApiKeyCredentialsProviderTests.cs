using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Options;
using NSubstitute;
using Trakx.Copper.ApiClient.Utils;
using Trakx.Utils.DateTimeHelpers;
using Trakx.Utils.Extensions;
using Xunit;

namespace Trakx.Copper.ApiClient.Tests.Unit
{
    public class ApiKeyCredentialsProviderTests
    {
        private readonly ApiKeyCredentialsProvider _apiKeyCredentials;
        private readonly CopperApiConfiguration _configuration;
        private readonly DateTimeOffset _timestamp;
        public ApiKeyCredentialsProviderTests()
        {
            _configuration = new CopperApiConfiguration { ApiKey = "apikeyPublic", ApiSecret = "c2FsdXRhdG91cw==", BaseUrl = "http://baseUrl.com" };
            var dateTimeProvider = Substitute.For<IDateTimeProvider>();
            _timestamp = DateTimeOffset.FromUnixTimeMilliseconds(123456789);
            dateTimeProvider.UtcNowAsOffset.ReturnsForAnyArgs(_timestamp);
            var options = Substitute.For<IOptions<CopperApiConfiguration>>();
            options.Value.ReturnsForAnyArgs(_configuration);
            _apiKeyCredentials = new ApiKeyCredentialsProvider(options, dateTimeProvider);
        }

        [Fact]
        public void Header_should_be_as_expected()
        {
            var message = new HttpRequestMessage(HttpMethod.Get, new Uri(_configuration.BaseUrl + "/path1/path2"));
            _apiKeyCredentials.AddCredentials(message);

            message.Headers.Authorization!.Scheme.Should().Be(ApiKeyCredentialsProvider.ApiKeyHeader);
            message.Headers.Authorization!.Parameter.Should().Be(_configuration.ApiKey);
            message.Headers.GetValues(ApiKeyCredentialsProvider.ApiTimestampHeader).Single()
                .Should().Be(_timestamp.ToUnixTimeMilliseconds().ToString());
            message.Headers.GetValues(ApiKeyCredentialsProvider.ApiSignatureHeader).Single().Should()
                .NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Signature_should_be_as_expected()
        {
            var message = new HttpRequestMessage(HttpMethod.Get, new Uri(_configuration.BaseUrl + "/path1/path2")) { Content = new StringContent("Hello, I'm the body of the request") };
            _apiKeyCredentials.AddCredentials(message);

            var retrievedSignature = message.Headers.GetValues(ApiKeyCredentialsProvider.ApiSignatureHeader).Single();
            await VerifySignature(message, retrievedSignature);
        }

        [Fact]
        public void Signature_should_depends_of_message_content()
        {
            var message1 = new HttpRequestMessage(HttpMethod.Put, new Uri(_configuration.BaseUrl + "/dev/now"));
            _apiKeyCredentials.AddCredentials(message1);
            var message2 = new HttpRequestMessage(HttpMethod.Post, new Uri(_configuration.BaseUrl + "/january"));
            _apiKeyCredentials.AddCredentials(message2);

            var signature1 = message1.Headers.GetValues(ApiKeyCredentialsProvider.ApiSignatureHeader).Single();
            var signature2 = message2.Headers.GetValues(ApiKeyCredentialsProvider.ApiSignatureHeader).Single();
            signature1.Should().NotBeEquivalentTo(signature2);
        }

        private async Task VerifySignature(HttpRequestMessage requestMessage, string retrievedSignature)
        {
            var body = await requestMessage.Content?.ReadAsStringAsync();
            var preHash = string.Concat(new List<string?>
            {
                _timestamp.ToUnixTimeMilliseconds().ToString(),
                requestMessage.Method.Method.ToUpperInvariant(),
                requestMessage.RequestUri?.AbsolutePath,
                body
            });
            var encodingSecret = Encoding.UTF8.GetBytes(_configuration.ApiSecret);
            var signature = new HMACSHA256(encodingSecret)
                .ComputeHash(Encoding.UTF8.GetBytes(preHash)).ToHexString();
            signature.Should().Be(retrievedSignature);
        }
    }
}
