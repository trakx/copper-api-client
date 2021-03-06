﻿using System;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Serilog;
using Trakx.Utils.Apis;
using Trakx.Utils.DateTimeHelpers;
using Trakx.Utils.Extensions;

namespace Trakx.Copper.ApiClient.Utils
{
    public interface ICopperCredentialsProvider : ICredentialsProvider { };
    public class ApiKeyCredentialsProvider : ICopperCredentialsProvider, IDisposable
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
            _encodingSecret = Encoding.UTF8.GetBytes(_configuration.ApiSecret);
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

        public Task AddCredentialsAsync(HttpRequestMessage msg)
        {
            AddCredentials(msg);
            return Task.CompletedTask;
        }

        #endregion

        private string GetTimestamp() => _dateTimeProvider.UtcNowAsOffset.ToUnixTimeMilliseconds()
            .ToString(CultureInfo.InvariantCulture);
        private string GetSignature(string preHash) => new HMACSHA256(_encodingSecret)
            .ComputeHash(Encoding.UTF8.GetBytes(preHash)).ToHexString();

        #region IDisposable

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            _tokenSource.Cancel();
            _tokenSource.Dispose();
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