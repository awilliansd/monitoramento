using Monitoramento.Hubs;
using Monitoramento.Hubs.Enum;
using Monitoramento.Infrastructure.Dto.WebAPI;
using Monitoramento.Infrastructure.Interfaces;
using Monitoramento.Services.WebAPI;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monitoramento.Jobs.Graficos
{
    public class ApiRequestsJob : IJobs
    {
        private readonly RequisicoesWebApiService _requisicoesService;
        private readonly IServerHubManager _serverHubManager;
        private readonly int HORAS_REQUISICOES;

        public ApiRequestsJob(RequisicoesWebApiService requisicoesService, IServerHubManager serverHubManager)
        {
            _requisicoesService = requisicoesService;
            _serverHubManager = serverHubManager;
            HORAS_REQUISICOES = 10;
        }

        [DisableConcurrentExecution(5)]
        [AutomaticRetry(Attempts = 0, LogEvents = false, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public Task Check(string channelName, IJobCancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var requisicoesApi = _requisicoesService.ObterPorPeriodo();
            var dataInicial = DateTime.Now;
            var requisicoes = requisicoesApi
                .GroupBy(x => x.Application)
                .Select(api => new LogApiDto
                {
                    APIRequisicao = api.Key,
                    Execucoes = ObterUltimas24Horas(requisicoesApi
                        .Where(x => x.Application == api.Key)
                        .Select(x => x)
                        .OrderBy(x => x.IntervaloID)
                        .ToList()),
                    Latencia = requisicoesApi.Any(x => x.Application == api.Key)
                        ? (long)requisicoesApi.Where(x => x.Application == api.Key)
                            .Average(a => (a.ResponseTimestamp - a.RequestTimestamp).TotalMilliseconds)
                        : 0
                }).ToList();

            if (!requisicoes.Any())
                requisicoes.Add(new LogApiDto
                {
                    APIRequisicao = "sem requisicao",
                    Execucoes = new int[10].ToList()
                });

            _serverHubManager.NotifyChart(EChartType.WebApi, "Requsições a Web Api",
                "Total Requisições: " + requisicoesApi.Count, requisicoes, CalcularEixoX(dataInicial));

            return Task.CompletedTask;
        }

        private List<string> CalcularEixoX(DateTime dataInicial)
        {
            var xLegendas = new string[HORAS_REQUISICOES];
            for (var i = 0; i < HORAS_REQUISICOES; i++)
            {
                var dataCalculada = dataInicial.AddHours(-i);
                xLegendas[HORAS_REQUISICOES - (i + 1)] = $"{dataCalculada.Hour}:00 - {dataCalculada.Hour}:59";
            }

            return xLegendas.ToList();
        }

        private List<int> ObterUltimas24Horas(ICollection<RequisicaoApiDto> requisicoes)
        {
            var ultimasRequisicoes = new int[HORAS_REQUISICOES];

            for (var i = HORAS_REQUISICOES; i > 0; i--)
            {
                var acesso = requisicoes.Where(x => x.IntervaloID == i).ToList();
                ultimasRequisicoes[i - 1] = acesso.Count;
            }

            return ultimasRequisicoes.ToList();
        }
    }
}