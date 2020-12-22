using System;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Microsoft.Extensions.Options;
using Serilog;
using Trakx.Utils.Api;

namespace Trakx.Copper.ApiClient.Utils
{
    public class ApiKeyCredentialsProvider : ICredentialsProvider, IDisposable
    {
        internal const string ApiKeyHeader = "ApiKey";
        internal const string ApiSignatureHeader = "X-Signature";
        internal const string ApiTimestampHeader = "X-Timestamp";

        private readonly CopperApiConfiguration _configuration;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly CancellationTokenSource _tokenSource;
        private readonly byte[] _encodingSecret;

        private static readonly ILogger Logger = Log.Logger.ForContext<ApiKeyCredentialsProvider>();

        public ApiKeyCredentialsProvider(IOptions<CopperApiConfiguration> configuration,
            IDateTimeProvider dateTimeProvider)
        {
            _configuration = configuration.Value;
            _dateTimeProvider = dateTimeProvider;

            _tokenSource = new CancellationTokenSource();
            _encodingSecret = Convert.FromBase64String(_configuration.ApiSecret);
        }


        #region Implementation of ICredentialsProvider

        /// <inheritdoc />
        public void AddCredentials(HttpRequestMessage msg)
        {
            var timestamp = GetTimestamp();
            var path = msg.RequestUri!.AbsolutePath;
            var method = msg.Method.Method.ToUpperInvariant();
            var body = msg.Content?.ReadAsStringAsync().GetAwaiter().GetResult() ?? string.Empty;

            var prehashString = timestamp + method + path + body;
            Logger.Verbose("PreHash string is {prehashString}", prehashString);

            msg.Headers.Authorization = new AuthenticationHeaderValue(ApiKeyHeader, _configuration.ApiKey);
            msg.Headers.Add(ApiTimestampHeader, timestamp);
            msg.Headers.Add(ApiSignatureHeader, GetSignature(prehashString));
            Logger.Verbose("Headers added");
        }
        #endregion

        private string GetTimestamp() => _dateTimeProvider.UtcNowAsOffset.ToUnixTimeMilliseconds()
            .ToString(CultureInfo.InvariantCulture);
        private string GetSignature(string preHash) => Convert.ToBase64String(new HMACSHA256(_encodingSecret)
            .ComputeHash(Encoding.UTF8.GetBytes(preHash)));

        #region IDisposable

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            _tokenSource.Cancel();
            _tokenSource?.Dispose();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}