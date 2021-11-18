using Monitoramento.Infrastructure.Dto.WebAPI;
using System.Collections.Generic;

namespace Monitoramento.Services.WebAPI
{
    public class RequisicoesWebApiService
    {
        public List<RequisicaoApiDto> ObterPorPeriodo()
        {
            return new List<RequisicaoApiDto> { new() { Id = 1, Application = "teste" } };
        }
    }
}