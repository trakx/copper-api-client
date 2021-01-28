using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;
using Serilog;

namespace Trakx.Copper.ApiClient
{
    public static partial class AddCopperClientExtension
    {
        private static void AddClients(this IServiceCollection services)
        {
            var delay = Backoff.DecorrelatedJitterBackoffV2(medianFirstRetryDelay: TimeSpan.FromMilliseconds(100), retryCount: 10, fastFirst: true);
                                    
            services.AddHttpClient<IOrdersClient, OrdersClient>("Trakx.Copper.ApiClient.OrdersClient")
                .AddPolicyHandler((s, request) => 
                    Policy<HttpResponseMessage>
                    .Handle<ApiException>()
                    .Or<HttpRequestException>()
                    .OrTransientHttpStatusCode()
                    .WaitAndRetryAsync(delay,
                        onRetry: (result, timeSpan, retryCount, context) =>
                        {
                            var logger = Log.Logger.ForContext<OrdersClient>();
                            LogFailure(logger, result, timeSpan, retryCount, context);
                        })
                    .WithPolicyKey("OrdersClient"));

                                
            services.AddHttpClient<IMessageSigningClient, MessageSigningClient>("Trakx.Copper.ApiClient.MessageSigningClient")
                .AddPolicyHandler((s, request) => 
                    Policy<HttpResponseMessage>
                    .Handle<ApiException>()
                    .Or<HttpRequestException>()
                    .OrTransientHttpStatusCode()
                    .WaitAndRetryAsync(delay,
                        onRetry: (result, timeSpan, retryCount, context) =>
                        {
                            var logger = Log.Logger.ForContext<MessageSigningClient>();
                            LogFailure(logger, result, timeSpan, retryCount, context);
                        })
                    .WithPolicyKey("MessageSigningClient"));

                                
            services.AddHttpClient<IAccountsClient, AccountsClient>("Trakx.Copper.ApiClient.AccountsClient")
                .AddPolicyHandler((s, request) => 
                    Policy<HttpResponseMessage>
                    .Handle<ApiException>()
                    .Or<HttpRequestException>()
                    .OrTransientHttpStatusCode()
                    .WaitAndRetryAsync(delay,
                        onRetry: (result, timeSpan, retryCount, context) =>
                        {
                            var logger = Log.Logger.ForContext<AccountsClient>();
                            LogFailure(logger, result, timeSpan, retryCount, context);
                        })
                    .WithPolicyKey("AccountsClient"));

                                
            services.AddHttpClient<IProxyWalletsClient, ProxyWalletsClient>("Trakx.Copper.ApiClient.ProxyWalletsClient")
                .AddPolicyHandler((s, request) => 
                    Policy<HttpResponseMessage>
                    .Handle<ApiException>()
                    .Or<HttpRequestException>()
                    .OrTransientHttpStatusCode()
                    .WaitAndRetryAsync(delay,
                        onRetry: (result, timeSpan, retryCount, context) =>
                        {
                            var logger = Log.Logger.ForContext<ProxyWalletsClient>();
                            LogFailure(logger, result, timeSpan, retryCount, context);
                        })
                    .WithPolicyKey("ProxyWalletsClient"));

        }
    }
}
