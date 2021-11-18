using Hangfire;
using System.Threading.Tasks;

namespace Monitoramento.Infrastructure.Interfaces
{
    public interface IJobs
    {
        //Checa jobs com tempo cronometrado e notifica cliente de sucesso ou erro.
        Task Check(string channelName, IJobCancellationToken cancellationToken);
    }
}