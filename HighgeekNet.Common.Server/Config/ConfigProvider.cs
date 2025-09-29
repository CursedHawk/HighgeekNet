using Microsoft.Extensions.Configuration;

namespace HighgeekNet.Common.Server.Config
{
    public class ConfigProvider
    {
        private static IConfiguration _config;

        public static void Configure(IConfiguration configuration)
        {
            _config = configuration;
        }

        public static string Get(string key)
        {
            return _config.GetSection(key).Value;
        }

        public static string GetConnectionString(string key)
        {
            return _config.GetConnectionString(key);
        }
    }
}
