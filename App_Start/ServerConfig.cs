using Microsoft.Extensions.Configuration;
using Steeltoe.Extensions.Configuration;

namespace ContosoUniversity
{
    public static class ServerConfig
    {
        public static IConfiguration GetCloudConfig()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddCloudFoundry()
                .Build();
        }
    }
}