using Monitoramento.Hubs.Enum;

namespace Monitoramento.Hubs
{
    public class Channel
    {
        public EHubChannel HubChannel { get; set; }
        public EChannelCategory Category { get; set; }
        public string ChannelName { get; set; }
        public string Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string UpdateTime { get; set; }
        public EStatusCode Code { get; set; }
        public string Message { get; set; }
    }
}