using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Trakx.Copper.ApiClient.Tests.Unit
{
    public class AddCopperClientExtensionTests
    {
        private readonly CopperApiConfiguration configuration;

        public AddCopperClientExtensionTests()
        {
            configuration = new CopperApiConfiguration
            {
                ApiKey = "abcdef1234",
                ApiSecret = "dnN6amRidmpza3Yg",
                BaseUrl = " https://api.copper.co"
            };
        }

        [Fact]
        public void Services_should_be_built()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddCopperClient(configuration);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            _ = serviceProvider.GetRequiredService<IAccountsClient>();
            _ = serviceProvider.GetRequiredService<IOrdersClient>();
        }
    }
}
