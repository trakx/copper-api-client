using Trakx.Utils.Attributes;

namespace Trakx.Copper.ApiClient
{
    public record CopperApiConfiguration
    {
#nullable disable
        public string BaseUrl { get; init; }

        [SecretEnvironmentVariable]
        public string ApiKey { get; init; }

        [SecretEnvironmentVariable]
        public string ApiSecret { get; init; }
#nullable restore
    }
}