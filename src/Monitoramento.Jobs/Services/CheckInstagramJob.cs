using Hangfire;
using Monitoramento.Hubs;
using Monitoramento.Hubs.Enum;
using Monitoramento.Hubs.Helper;
using Monitoramento.Infrastructure.Interfaces;
using Monitoramento.Infrastructure.Util;
using Monitoramento.WebUtility;
using RestSharp;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Monitoramento.Jobs.Services
{
    public class CheckInstagramJob : IJobs
    {
        private readonly IServerHubManager _serverHubManager;
        private readonly UrlApiConfig _urlApiConfig;

        public CheckInstagramJob(IServerHubManager serverHubManager, UrlApiConfig urlApiConfig)
        {
            _serverHubManager = serverHubManager;
            _urlApiConfig = urlApiConfig;
        }

        public async Task Check(string channelName, IJobCancellationToken cancellationToken)
        {
            var startTime = SystemTime.Now();
            var time = new TimeElapsed();
            
            try
            {
                var client = new RestClient(_urlApiConfig.UrlInstagram);
                var request = new RestRequest(Method.GET);
                request.AddHeader("cache-control", "no-cache");

                var response = await client.ExecuteAsync(request);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    _serverHubManager.Notify(EHubChannel.InstagramSite, EChannelCategory.Service,
                        channelName, EStatusCode.Fail, $"Requisição falhou com codigo: {response.StatusCode}",
                        startTime);
                    return;
                }

                if (!response.StatusDescription.Contains("OK"))
                {
                    _serverHubManager.Notify(EHubChannel.InstagramSite, EChannelCategory.Service,
                        channelName, EStatusCode.Fail, "Requisição falhou ao pesquisar", startTime);
                    return;
                }

                if (time.GetElapsedTimeBySecond() > 10)
                {
                    _serverHubManager.Notify(EHubChannel.InstagramSite, EChannelCategory.Service, channelName,
                        EStatusCode.Warning, $"Sucesso com {time.GetElapsedTimeBySecond()} segundos de atraso",
                        startTime);
                }
                else
                {
                    _serverHubManager.Notify(EHubChannel.InstagramSite, EChannelCategory.Service, channelName, EStatusCode.Ok,
                        string.Empty, startTime);
                }
            }
            catch (Exception)
            {
                _serverHubManager.Notify(EHubChannel.InstagramSite, EChannelCategory.Service, channelName,
                    EStatusCode.Fail, "Erro ao pesquisar", startTime);
                throw;
            }
        }
    }
}