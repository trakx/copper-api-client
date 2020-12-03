using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Trakx.Copper.ApiClient.Tests.Integration
{
    public class OrdersClientTests: CopperClientTestsBase
    {
        private readonly IOrdersClient _ordersClient;

        public OrdersClientTests(CopperApiFixture apiFixture, ITestOutputHelper output) : base(apiFixture, output)
        {
            _ordersClient = ServiceProvider.GetRequiredService<IOrdersClient>();
        }
        //to implement after cleaned the openApi
    }
}
