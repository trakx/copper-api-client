
namespace Trakx.Copper.ApiClient
{
    internal partial class OrdersClient
    {
        partial void PrepareRequest(System.Net.Http.HttpClient client, System.Net.Http.HttpRequestMessage request, string url)
        {
            CredentialProvider.AddCredentials(request);
        }
    }
    internal partial class MessageSigningClient
    {
        partial void PrepareRequest(System.Net.Http.HttpClient client, System.Net.Http.HttpRequestMessage request, string url)
        {
            CredentialProvider.AddCredentials(request);
        }
    }
    internal partial class AccountsClient
    {
        partial void PrepareRequest(System.Net.Http.HttpClient client, System.Net.Http.HttpRequestMessage request, string url)
        {
            CredentialProvider.AddCredentials(request);
        }
    }
    internal partial class ProxyWalletsClient
    {
        partial void PrepareRequest(System.Net.Http.HttpClient client, System.Net.Http.HttpRequestMessage request, string url)
        {
            CredentialProvider.AddCredentials(request);
        }
    }

}