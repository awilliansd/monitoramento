using Hangfire;
using Monitoramento.Configuration.Configurations;
using Monitoramento.Configuration.HangfireConfig;
using Monitoramento.Configuration.SwaggerConfig;
using Monitoramento.ErrorHandling;
using Monitoramento.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Monitoramento.Configuration.DependencyInjectionConfig;

namespace Monitoramento
{
    public class Startup
    {
        private IConfigurationRoot Configuration { get; }

        public Startup(IWebHostEnvironment env)
        {
            Configuration = env.ConfigurationEnvironmentConfig();
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.AddConfigurationHangfire();
            app.UseStaticFiles();

            if (env.IsDevelopment() || env.IsStaging())
                app.AddConfigurationSwaggerUI();

            app.UseExceptionHandler(new ExceptionHandlerOptions
            {
                ExceptionHandler = new ConvertExceptionToJson(loggerFactory).Invoke
            });

            app.UseRouting();
            app.UseCors("AllowAnyOrigin");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ServerStatusHub>("/serverStatusHub");
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHangfireDashboard();
            });

            HangfireInitilize.IniciarHangfireJobs();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddConfigurationServices(Configuration);
            services.AddConfigurationSwaggerGen();
            services.AddDependencyInjection(Configuration);
        }
    }
}