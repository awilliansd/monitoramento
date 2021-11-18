using Monitoramento.Hubs;
using Monitoramento.Hubs.Enum;
using Monitoramento.Hubs.Helper;
using Monitoramento.Infrastructure.Interfaces;
using Monitoramento.Infrastructure.Util;
using Hangfire;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Monitoramento.Jobs.Procedures
{
    public class TarefasDiariasJob : IJobs
    {
        private static string _channelName;
        private readonly IServerHubManager _serverHubManager;
        private IConfiguration Configuration { get; }

        public TarefasDiariasJob(IServerHubManager serverHubManager, IConfiguration configuration)
        {
            _serverHubManager = serverHubManager;
            Configuration = configuration;
        }

        [DisableConcurrentExecution(10)]
        [AutomaticRetry(Attempts = 0, LogEvents = false, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public Task Check(string channelName, IJobCancellationToken cancellationToken)
        {
            var startTime = SystemTime.Now();
            var time = new TimeElapsed();
            _channelName = channelName;
            var errorMessages = new StringBuilder();
            var timeProcedure = new TimeElapsed();
            var tempoExecucao = string.Empty;

            try
            {
                using (var conn = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
                using (var command = new SqlCommand { CommandType = CommandType.StoredProcedure, CommandTimeout = 600 })
                {
                    command.Connection = conn;
                    conn.Open();

                    command.CommandText = "LimparLog";
                    command.ExecuteNonQuery();
                    tempoExecucao += "1. LimparLog - " + timeProcedure.GetTimeByFormat() + "<br/>";

                    timeProcedure.StopTime();
                }

                tempoExecucao += "Banco: Monitoramento <br/>";
                tempoExecucao += "Tempo de Execução: " + time.GetTimeByFormatFinal() + "<br/>";
                tempoExecucao += "Periodicidade: Diário 03:00";

                if (time.GetSecond() > 420)
                    Notify(EStatusCode.Warning, $"Sucesso com atraso. <br/>" + tempoExecucao, startTime);
                else
                    Notify(EStatusCode.Ok, tempoExecucao, startTime);

                return Task.CompletedTask;
            }
            catch (SqlException ex)
            {
                for (var i = 0; i < ex.Errors.Count; i++)
                {
                    errorMessages.Append("Index #" + i + "<br/>" +
                                         "Mensagem: " + ex.Errors[i].Message + "<br/>" +
                                         "Linha: " + ex.Errors[i].LineNumber + "<br/>" +
                                         "Source: " + ex.Errors[i].Source + "<br/>" +
                                         "Procedure: " + ex.Errors[i].Procedure + "<br/>");
                }

                Notify(EStatusCode.Fail, errorMessages.ToString(), startTime);
                throw;
            }
            catch (Exception ex)
            {
                Notify(EStatusCode.Fail, ex.Message, startTime);
                throw;
            }
        }

        private void Notify(EStatusCode code, string message, DateTime startTime)
        {
            _serverHubManager.Notify(EHubChannel.TarefasDiarias, EChannelCategory.Procedure, _channelName, code,
                message, startTime);
        }
    }
}