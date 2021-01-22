using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Trakx.Copper.ApiClient.Tests.Integration
{
    public class AccountsClientTests : CopperClientTestsBase
    {
        private readonly IAccountsClient _accountsClient;

        public AccountsClientTests(CopperApiFixture apiFixture, ITestOutputHelper output) : base(apiFixture, output)
        {
            _accountsClient = ServiceProvider.GetRequiredService<IAccountsClient>();
        }

        [Fact]
        public async Task GetAccount_should_send_back_all_accounts()
        {
            var response = await _accountsClient.GetAccountsAsync(); 
            response.Result.Accounts.Should().NotBeNullOrEmpty();
            response.Result.Accounts[0].OrganizationId.Should().Be("TRAK");
            response.Result.Accounts[0]._embedded.Wallets.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GetWallets_should_send_back_all_wallets()
        {
            var response = await _accountsClient.GetWalletsAsync();
            response.Result.Wallets.Should().NotBeNullOrEmpty();
            response.Result.Wallets[0].Balance.Should().BeGreaterOrEqualTo(0.001);
        }
    }
}
