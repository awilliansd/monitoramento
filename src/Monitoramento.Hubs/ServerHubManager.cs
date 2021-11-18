using Monitoramento.Hubs.Enum;
using Monitoramento.Infrastructure.Dto.AcessosUsuario;
using Monitoramento.Infrastructure.Util;
using Microsoft.AspNetCore.SignalR;
using Monitoramento.Infrastructure.Dto.WebAPI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Monitoramento.Hubs
{
    public class ServerHubManager : IServerHubManager
    {
        private readonly IHubContext<ServerStatusHub> _hubContext;

        public ServerHubManager(IHubContext<ServerStatusHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public void NotifyChart(EChartType chartType, string titulo, string subTitulo,
            ICollection<LogApiDto> requisicoes, List<string> xLegenda)
        {
            var data = SystemTime.Now();
            var notification = new ChartNotification
            {
                ChartType = chartType,
                Titulo = titulo,
                SubTitulo = subTitulo,
                Requisicoes = requisicoes,
                XLegenda = xLegenda,
                UpdateTime = data.ToString("dd/MM/yyyy HH:mm:ss")
            };

            _hubContext.Clients.All.SendAsync("notifyLogAPI", notification);
        }

        public void NotifyChart(EChartType chartType, string titulo, string subTitulo,
            ICollection<UsuarioRequisicaoDto> usuarios, List<string> xLegenda)
        {
            var data = SystemTime.Now();
            var notification = new ChartNotification
            {
                ChartType = chartType,
                Titulo = titulo,
                SubTitulo = subTitulo,
                Usuarios = usuarios,
                XLegenda = xLegenda,
                UpdateTime = data.ToString("dd/MM/yyyy HH:mm:ss")
            };

            _hubContext.Clients.All.SendAsync("notifyUsuario", notification);
        }

        public void Notify(EHubChannel hubChannel, EChannelCategory category, string title, EStatusCode code,
            string message, DateTime timeInicio)
        {
            var data = SystemTime.Now();
            var notification = new Channel
            {
                HubChannel = hubChannel,
                ChannelName = title,
                Message = message,
                Code = code,
                Category = category,
                Date = data.ToString("dd/MM/yyyy"),
                UpdateTime = data.ToString("dd/MM/yyyy HH:mm:ss"),
                StartTime = timeInicio.ToString("HH:mm:ss"),
                EndTime = data.ToString("HH:mm:ss")
            };

            var channel = ServerListManager.GetChannels().FirstOrDefault(f => f.HubChannel == hubChannel);

            if (channel == null)
            {
                ServerListManager.SetChannels(notification);
                _hubContext.Clients.All.SendAsync("notify", notification);
            }
            else
            {
                ServerListManager.UpdateChannels(channel, notification);
                _hubContext.Clients.All.SendAsync("notify", notification);
            }
        }
    }
}