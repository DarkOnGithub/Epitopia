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
            Debug.Log(connectionInfos.ClientId);
            if (connectionInfos.ClientId == NetworkManager.Singleton.LocalClientId)
            {
                LocalPlayer = player;
                LocalPlayer.World = WorldManager.GetWorld(WorldIdentifier.Overworld);
            }

            if (NetworkManager.Singleton.IsHost)
            {
                SetPlayerInfos(player);
                var height = WorldManager.GetWorld(WorldIdentifier.Overworld).WorldGenerator.GetHeightAt(0);
                GameObject spawnedObject = NetworkManager.Instantiate(Resources.Load<GameObject>("Sprites/MainChar/PrefabChar/PlayerSwitchSide"), new Vector3(0, height, 0), Quaternion.identity);
                NetworkObject networkObject = spawnedObject.GetComponent<NetworkObject>();
                spawnedObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                networkObject.SpawnAsPlayerObject(connectionInfos.ClientId);
                LocalPlayer.Position = new Vector3(0, height, 0);
                await Task.Delay(2000);
                spawnedObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;

            }
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