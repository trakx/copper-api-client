using Trakx.Utils.Attributes;

namespace Trakx.Copper.ApiClient
{
    public class CopperApiConfiguration
    {
#nullable disable
        [SecretEnvironmentVariable("CopperApiConfiguration__BaseUrl")]
        public string BaseUrl { get; set; }

        [SecretEnvironmentVariable("CopperApiConfiguration__ApiKey")]
        public string ApiKey { get; set; }

        [SecretEnvironmentVariable("CopperApiConfiguration__ApiSecret")]
        public string ApiSecret { get; set; }
#nullable restore
    }
}