using System.Collections.Generic;
using Network.Messages.Packets.Network;
using Unity.Netcode;
using World;

namespace Players
{
    public static class PlayerManager
    {
        public static Player LocalPlayer { get; set; }
        public static List<Player> Players = new();
        public static void OnPlayerConnected(ConnectionMessage connectionInfos)
        {
            var player = new Player(connectionInfos.PlayerName, connectionInfos.PlayerId, connectionInfos.ClientId); 

            if(connectionInfos.ClientId == NetworkManager.Singleton.LocalClientId)
            {
                LocalPlayer = player;
                LocalPlayer.World = WorldManager.GetWorld(WorldIdentifier.Overworld);
            }
            if(NetworkManager.Singleton.IsHost)
                SetPlayerInfos(player);    
        }
        
        public static void OnPlayerDisconnected(ConnectionMessage player)
        {
            Players.RemoveAll(p => p.ClientId == player.ClientId);
        }

        private static void SetPlayerInfos(Player player)
        {
            player.World = WorldManager.GetWorld(WorldIdentifier.Overworld);
        }
    }
}