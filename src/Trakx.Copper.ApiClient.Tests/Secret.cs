using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trakx.Copper.ApiClient.Tests
{
    public static class Secrets
    {
        static Secrets()
        {
            var srcPath = new DirectoryInfo(Environment.CurrentDirectory).Parent?.Parent?.Parent?.Parent;
            try
            {
                DotNetEnv.Env.Load(Path.Combine(srcPath?.FullName ?? string.Empty, ".env"));
            }
            catch (Exception)
            {
                // Fail to load the file on the CI pipeline, it should have environment variables defined.
            }
        }

        public static string? ExchangeApiKey => Environment.GetEnvironmentVariable("CopperApiConfiguration__ApiKey");
        public static string? ExchangeApiSecret => Environment.GetEnvironmentVariable("CopperApiConfiguration__ApiSecret");
    }
}
