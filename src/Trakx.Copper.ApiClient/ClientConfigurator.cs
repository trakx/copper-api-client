using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Trakx.Copper.ApiClient.Utils;

namespace Trakx.Copper.ApiClient
{
    internal class ClientConfigurator
    {
        private readonly IServiceProvider _serviceProvider;

        public ClientConfigurator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            ApiConfiguration = serviceProvider.GetService<IOptions<CopperApiConfiguration>>()!.Value;
        }

        public CopperApiConfiguration ApiConfiguration { get; }

        public ICredentialsProvider GetCredentialProvider(Type clientType)
        {
            switch (clientType.Name)
            {
                case nameof(MarketDataClient):
                //case nameof(ExchangesClient):
                    return new NoCredentialsProvider();
                default:
                    return _serviceProvider.GetService<ICredentialsProvider>()!;
            }
        }
    }
}