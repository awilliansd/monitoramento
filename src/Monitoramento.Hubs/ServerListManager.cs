using Monitoramento.Infrastructure.Dto.AcessosUsuario;
using System.Collections.Generic;

namespace Monitoramento.Hubs
{
    public static class ServerListManager
    {
        private static List<Channel> _channelList = new();
        private static List<UsuarioRequisicaoDto> _UsuariosDtoList = new();

        public static List<Channel> GetChannels()
        {
            return _channelList;
        }

        public static List<UsuarioRequisicaoDto> GetCharts()
        {
            return _UsuariosDtoList;
        }

        public static void SetChannels(Channel channel)
        {
            _channelList.Add(channel);
        }

        public static void UpdateChannels(Channel channel, Channel notification)
        {
            _channelList[_channelList.IndexOf(channel)] = notification;
        }
    }
}