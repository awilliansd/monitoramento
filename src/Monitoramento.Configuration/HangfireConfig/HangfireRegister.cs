using Monitoramento.Integration.API;
using Monitoramento.Jobs.Graficos;
using Monitoramento.Jobs.Procedures;
using Hangfire;
using Monitoramento.Jobs.Services;
using System;
using System.Runtime.InteropServices;

namespace Monitoramento.Configuration.HangfireConfig
{
    public class HangfireRegister
    {
        private const string Tempo10Min = "0 */10 * ? * *";
        private const string Tempo2Min = "0 */2 * ? * *";
        private const string Tempo30Segundos = "0/30 * * ? * *";

        private static readonly TimeZoneInfo TimeZone =
            IsLinux() ? TimeZoneInfo.FindSystemTimeZoneById("America/Campo_Grande") : TimeZoneInfo.Local;

        public static void Register()
        {
            GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 1 });

            RegisterSensitiveJobs();
            RegisterRecurringJobs();
        }

        private static bool IsLinux() => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        /// <summary>
        /// Registra serviços sensíveis, que manipulam dados da base, a serem executadas com periodicidade.
        /// RemoveIfExists: metodo garante que o serviço não seja executado quando a aplicação é inicializada.
        /// </summary>
        private static void RegisterSensitiveJobs()
        {
            const string jobTarefasId = "Tarefas Diárias";
            RecurringJob.RemoveIfExists(jobTarefasId);
            RecurringJob.AddOrUpdate<TarefasDiariasJob>(jobTarefasId,
                job => job.Check(jobTarefasId, JobCancellationToken.Null), Cron.Daily(3), TimeZone);
        }

        /// <summary>
        /// Registra serviços a serem executadas com periodicidade a cargo de informação.
        /// </summary>
        private static void RegisterRecurringJobs()
        {
            // Recurring Jobs
            // Sites
            RecurringJob.AddOrUpdate<CheckGoogleJob>(
                "Site - Google Brasil",
                job => job.Check("Site - Google Brasil", JobCancellationToken.Null), Tempo2Min,
                TimeZone);
            
            RecurringJob.AddOrUpdate<CheckInstagramJob>(
                "Site - Instagram",
                job => job.Check("Site - Instagram", JobCancellationToken.Null), Tempo2Min,
                TimeZone);

            // Graficos
            RecurringJob.AddOrUpdate<ApiRequestsJob>("Requisições API",
                job => job.Check("Requisições API", JobCancellationToken.Null), Cron.Hourly(59), TimeZone);

            // API
            RecurringJob.AddOrUpdate<BonificacaoAPIIntegration>("Docker - Bonificação",
                job => job.Check("Docker - Bonificação", JobCancellationToken.Null), Tempo2Min, TimeZone);
            
            // Fire and forget jobs
            // var jobId = BackgroundJob.Enqueue(() => Console.WriteLine("Fire-and-forget!"));
            
            // Delayed jobs
            //var jobId = BackgroundJob.Schedule(() => Console.WriteLine("Delayed!"),TimeSpan.FromDays(7));
            
            // Continuations
            // BackgroundJob.ContinueWith(jobId, () => Console.WriteLine("Continuation!"));
        }
    }
}