using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Steeltoe.Extensions.Configuration;

namespace ContosoUniversity
{
    public static class ServerConfig
    {
        public static IConfiguration Configuration { get; private set; }

        public static void RegisterCloudConfig()
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddCloudFoundry()
                .Build();
        }
    }
}