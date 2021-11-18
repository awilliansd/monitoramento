using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Monitoramento.Configuration.Configurations
{
    public static class ServicesSettings
    {
        public static void AddConfigurationServices(this IServiceCollection services, IConfigurationRoot configRoot)
        {
            services.AddCors(option => option.AddPolicy("AllowAnyOrigin",
                builder =>
                {
                    builder.AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowAnyOrigin();
                }));

            services.AddMvc()
                .AddControllersAsServices();

            services.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true;
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
                o.ApiVersionReader = new MediaTypeApiVersionReader();
            });

            services.Configure<KestrelServerOptions>(serverOptions => { serverOptions.AllowSynchronousIO = true; });
            
            services.AddSignalR(options => options.EnableDetailedErrors = true);
        }
    }
}