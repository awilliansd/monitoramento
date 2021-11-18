using Monitoramento.Hubs.Enum;
using Monitoramento.Infrastructure.Dto.WebAPI;
using System;
using System.Collections.Generic;

namespace Monitoramento.Hubs
{
    public interface IServerHubManager
    {
        void NotifyChart(EChartType chartType, string titulo, string subTitulo, ICollection<LogApiDto> requisicoes,
            List<string> xLegenda);

        void Notify(EHubChannel hubChannel, EChannelCategory category, string title, EStatusCode code, string message,
            DateTime timeInicio);
    }
}