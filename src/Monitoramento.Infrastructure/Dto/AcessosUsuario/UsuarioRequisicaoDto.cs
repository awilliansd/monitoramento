using System.Collections.Generic;

namespace Monitoramento.Infrastructure.Dto.AcessosUsuario
{
    public class UsuarioRequisicaoDto
    {
        public string Perfil { get; set; }
        public ICollection<int> Execucoes { get; set; }
    }
}