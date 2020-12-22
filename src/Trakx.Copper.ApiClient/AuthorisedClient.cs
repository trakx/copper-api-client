using Trakx.Utils.Api;

namespace Trakx.Copper.ApiClient
{
    internal abstract class AuthorisedClient 
    {
        protected readonly ICredentialsProvider CredentialProvider;
        protected string BaseUrl { get; }

        protected AuthorisedClient(ClientConfigurator clientConfigurator) 
        {
            CredentialProvider = clientConfigurator.GetCredentialProvider(GetType());
            BaseUrl = clientConfigurator.ApiConfiguration.BaseUrl;
        }
    }
}