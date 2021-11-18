using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Monitoramento.Hubs;
using Monitoramento.Infrastructure.Interfaces;
using Monitoramento.Services.Repository.Factory;
using Monitoramento.WebUtility;
using System.Reflection;

namespace Monitoramento.Configuration.DependencyInjectionConfig
{
    public static class DependencyInjection
    {
        public static void AddDependencyInjection(this IServiceCollection services, IConfigurationRoot configRoot)
        {
            var sessionFactory = new SessionFactory().BuildSessionFactory(configRoot.GetConnectionString("DefaultConnection"),
                "Monitoramento.Services.Repository.Mappings.WebAPI");
            
            services.AddSingleton(_ => sessionFactory.OpenSession());
            services.AddHangfireServer();
            services.AddHangfire(config =>
                config.UsePostgreSqlStorage(configRoot.GetConnectionString("DefaultConnection")));

            var urlApiConfig = configRoot.GetSection("UrlApiConfig").Get<UrlApiConfig>();
            services.AddSingleton(_ => urlApiConfig);
            
            services.AddSingleton<IServerHubManager, ServerHubManager>();
            
            services.Scan(x =>
            {
                x.FromAssemblies(typeof(IJobs).GetTypeInfo().Assembly)
                    .AddClasses(classes => classes.AssignableTo<IJobs>())
                    .AsImplementedInterfaces()
                    .WithScopedLifetime();
            });
            
            services.Scan(x =>
            {
                x.FromAssemblies(typeof(IIntegration).GetTypeInfo().Assembly)
                    .AddClasses(classes => classes.AssignableTo<IIntegration>())
                    .AsImplementedInterfaces()
                    .WithScopedLifetime();
            });
            
            services.AddScoped<IServerHubManager, ServerHubManager>();
        }
    }
}