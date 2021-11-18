using Monitoramento.Hubs.Enum;
using Monitoramento.Infrastructure.Dto.AcessosUsuario;
using Monitoramento.Infrastructure.Dto.WebAPI;
using System.Collections.Generic;

namespace Monitoramento.Hubs
{
    public class ChartNotification
    {
        public EChartType ChartType { get; set; }
        public string Titulo { get; set; }
        public string SubTitulo { get; set; }
        public ICollection<string> XLegenda { get; set; }
        public ICollection<LogApiDto> Requisicoes { get; internal set; }
        public ICollection<UsuarioRequisicaoDto> Usuarios { get; internal set; }
        public string UpdateTime { get; set; }
    }
}