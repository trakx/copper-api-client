using System;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Xunit;
using Xunit.Abstractions;

namespace Trakx.Copper.ApiClient.Tests.Integration
{
    [Collection(nameof(ApiTestCollection))]
    public class CopperClientTestsBase
    {
        protected ServiceProvider ServiceProvider;
        protected ILogger Logger;

        public CopperClientTestsBase(CopperApiFixture apiFixture, ITestOutputHelper output)
        {
            Logger = new LoggerConfiguration().WriteTo.TestOutput(output).CreateLogger();

            ServiceProvider = apiFixture.ServiceProvider;
        }
    }

    [CollectionDefinition(nameof(ApiTestCollection))]
    public class ApiTestCollection : ICollectionFixture<CopperApiFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }

    public class CopperApiFixture : IDisposable
    {
        public ServiceProvider ServiceProvider;

        public CopperApiFixture()
        {
            var configuration = new CopperApiConfiguration()
            {
                ApiKey = Secrets.ExchangeApiKey,
                ApiSecret = Secrets.ExchangeApiSecret,
                BaseUrl = "https://api.copper.co"
            };

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton(configuration);
            serviceCollection.AddCopperClient(configuration);

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            ServiceProvider.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
