using System;
using System.IO;
using Trakx.Utils.Extensions;

namespace Trakx.Copper.ApiClient.Tests
{
    public static class Secrets
    {
        static Secrets()
        {
            var isRootDirectory = DirectoryInfoExtensions.TryWalkBackToRepositoryRoot(null, out var rootDirectory);
            if (!isRootDirectory)
                rootDirectory = null;
            try
            {
                DotNetEnv.Env.Load(Path.Combine(rootDirectory?.FullName ?? string.Empty, "src", ".env"));
            }
            catch (Exception)
            {
                // Fail to load the file on the CI pipeline, it should have environment variables defined.
            }
        }

        public static string CopperApiKey => Environment.GetEnvironmentVariable("CopperApiConfiguration__ApiKey")!;
        public static string CopperApiSecret => Environment.GetEnvironmentVariable("CopperApiConfiguration__ApiSecret")!;
    }
}
