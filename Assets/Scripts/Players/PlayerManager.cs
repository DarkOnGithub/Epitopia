using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Network.Messages.Packets.Network;
using Unity.Netcode;
using UnityEngine;
using World;

namespace Players
{
    public static class PlayerManager
    {
        public static List<Player> Players = new();
        public static Player LocalPlayer { get; set; }

        public static async void OnPlayerConnected(ConnectionMessage connectionInfos)
        {
            var player = new Player(connectionInfos.PlayerName, connectionInfos.PlayerId, connectionInfos.ClientId);
            if (connectionInfos.ClientId == NetworkManager.Singleton.LocalClientId)
                LocalPlayer = player;

            if (NetworkManager.Singleton.IsHost) await SetPlayerInfos(player);
            Scanner.Instance.InitializeScanner(UnityEngine.Camera.main);

        }

        public static void OnPlayerDisconnected(ConnectionMessage player)
        {
            Players.RemoveAll(p => p.ClientId == player.ClientId);
        }

        private static async Task SetPlayerInfos(Player player)
        {
            player.World = WorldManager.GetWorld(WorldIdentifier.Overworld);
        }
    }
}