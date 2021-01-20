using Trakx.Utils.Attributes;
using Trakx.Utils.Testing;

namespace Trakx.Copper.ApiClient.Tests
{
    public record Secrets :SecretsBase
    {
        [SecretEnvironmentVariable(nameof(CopperApiConfiguration), nameof(CopperApiConfiguration.ApiKey))]
        public string CopperApiKey { get; init; }

        [SecretEnvironmentVariable(nameof(CopperApiConfiguration), nameof(CopperApiConfiguration.ApiSecret))]
        public string CopperApiSecret { get; init; }
    }
}
