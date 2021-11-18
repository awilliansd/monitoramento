using System;

namespace Monitoramento.Infrastructure.Dto.WebAPI
{
    public class RequisicaoApiDto
    {
        public int Id { get; set; }
        public int IntervaloID { get; set; }
        public string Application { get; set; }
        public DateTime RequestTimestamp { get; set; }
        public DateTime ResponseTimestamp { get; set; }
    }
}