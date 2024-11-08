using System.Collections.Generic;
using Network.Messages.Packets.Network;
using Unity.Netcode;

namespace Players
{
    public static class PlayerManager
    {
        public static Player LocalPlayer { get; set; }
        public static List<Player> Players = new();
        public static void OnPlayerConnected(ConnectionMessage connectionInfos)
        {
            if(connectionInfos.ClientId == NetworkManager.ServerClientId)
            {
                var player = new Player(connectionInfos.PlayerName, connectionInfos.PlayerId, connectionInfos.ClientId); 
                return;
            }
        }
        
        public static void OnPlayerDisconnected(ConnectionMessage player)
        {
        }
    }
}