using Trakx.Utils.Attributes;

namespace Trakx.Copper.ApiClient
{
    public class CopperApiConfiguration
    {
#nullable disable
        public string BaseUrl { get; set; }

        [SecretEnvironmentVariable]
        public string ApiKey { get; set; }

        [SecretEnvironmentVariable]
        public string ApiSecret { get; set; }
#nullable restore
    }
}