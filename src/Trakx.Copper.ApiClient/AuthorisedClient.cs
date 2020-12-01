using Trakx.Copper.ApiClient.Utils;

namespace Trakx.Copper.ApiClient
{
    internal abstract class AuthorisedClient : FavouriteExchangesClient
    {
        protected readonly ICredentialsProvider CredentialProvider;
        protected string BaseUrl { get; }

        protected AuthorisedClient(ClientConfigurator clientConfigurator) : base(clientConfigurator)
        {
            CredentialProvider = clientConfigurator.GetCredentialProvider(GetType());
            BaseUrl = clientConfigurator.ApiConfiguration.BaseUrl;
        }
    }
}