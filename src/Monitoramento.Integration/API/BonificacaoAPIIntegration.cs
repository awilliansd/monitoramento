using Monitoramento.Hubs;
using Monitoramento.Hubs.Enum;
using Monitoramento.Hubs.Helper;
using Monitoramento.Infrastructure.Dto;
using Monitoramento.Infrastructure.Interfaces;
using Monitoramento.Infrastructure.Util;
using Monitoramento.WebUtility;
using Hangfire;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Monitoramento.Integration.API
{
    public class BonificacaoAPIIntegration : IIntegration
    {
        private readonly IServerHubManager _serverHubManager;
        private readonly UrlApiConfig _urlApiConfig;

        public BonificacaoAPIIntegration(IServerHubManager serverHubManager, UrlApiConfig urlApiConfig)
        {
            _serverHubManager = serverHubManager;
            _urlApiConfig = urlApiConfig;
        }

        public async Task Check(string channelName, IJobCancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var startTime = SystemTime.Now();
            var time = new TimeElapsed();

            try
            {
                var client = new RestClient(_urlApiConfig.UrlApiBonificacao);
                var request = new RestRequest(Method.GET);
                request.AddHeader("cache-control", "no-cache");

                var response = await client.ExecuteAsync(request);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    _serverHubManager.Notify(EHubChannel.APIBonificacao, EChannelCategory.Internal, channelName,
                        EStatusCode.Fail, $"Requisição falhou com codigo: {response.StatusCode}", startTime);
                    return;
                }

                var retorno = response.Content;

                if (!retorno.Any())
                {
                    _serverHubManager.Notify(EHubChannel.APIBonificacao, EChannelCategory.Internal, channelName,
                        EStatusCode.Fail, "Requisição falhou ao pesquisar", startTime);
                    return;
                }

                if (time.GetElapsedTimeBySecond() > 10)
                    _serverHubManager.Notify(EHubChannel.APIBonificacao, EChannelCategory.Internal, channelName,
                        EStatusCode.Warning, $"Sucesso com {time.GetElapsedTimeBySecond()} segundos de atraso",
                        startTime);
                else
                    _serverHubManager.Notify(EHubChannel.APIBonificacao, EChannelCategory.Internal, channelName, EStatusCode.Ok,
                        string.Empty, startTime);
            }
            catch (Exception)
            {
                _serverHubManager.Notify(EHubChannel.APIBonificacao, EChannelCategory.Internal, channelName, EStatusCode.Fail,
                    "Erro ao pesquisar", startTime);
                throw;
            }
        }
    }
}