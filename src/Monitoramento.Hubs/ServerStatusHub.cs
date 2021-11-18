using Microsoft.AspNetCore.SignalR;

namespace Monitoramento.Hubs
{
    public class ServerStatusHub : Hub
    {
        public void ChannelList(string id)
        {
            Clients.Client(id).SendAsync("sendChannels", ServerListManager.GetChannels());
        }

        public void ChartList(string id)
        {
            Clients.Client(id).SendAsync("sendCharts", ServerListManager.GetCharts());
        }

        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }
    }
}