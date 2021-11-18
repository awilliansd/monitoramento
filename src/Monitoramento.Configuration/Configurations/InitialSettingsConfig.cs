using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Monitoramento.Configuration.Configurations
{
    public static class InitialSettingsConfig
    {
        public static IConfigurationRoot ConfigurationEnvironmentConfig(this IWebHostEnvironment env)
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();

            return configBuilder.Build();
        }
    }
}