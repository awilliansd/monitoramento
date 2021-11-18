using Hangfire;
using Microsoft.AspNetCore.Builder;

namespace Monitoramento.Configuration.HangfireConfig
{
    public static class HangfireInitilize
    {
        public static void AddConfigurationHangfire(this IApplicationBuilder app)
        {
            //Registering schedules
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthorizationFilter() },
            });
            
            HangfireRegister.Register();
        }

        public static void IniciarHangfireJobs()
        {
            RecurringJob.Trigger("Usuarios Logados");
            RecurringJob.Trigger("Requisições API");
        }
    }
}