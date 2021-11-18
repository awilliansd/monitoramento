using System.Collections.Generic;

namespace Monitoramento.Infrastructure.Dto.WebAPI
{
    public class LogApiDto
    {
        public string APIRequisicao { get; set; }

        public long Latencia { get; set; }

        public ICollection<int> Execucoes { get; set; }
    }
}