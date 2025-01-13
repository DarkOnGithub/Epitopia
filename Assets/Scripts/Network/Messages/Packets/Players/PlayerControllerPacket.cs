using MessagePack;
using Unity.Netcode;
using UnityEngine;

namespace Network.Messages.Packets.Players
{
    
    [MessagePackObject]
    public struct PlayerControllerMessage : IMessageData
    {
        [Key(0)]
        public Vector2 Scale;

        [Key(1)] public ulong NetworkObjectId;
    }
    public class PlayerControllerPacket : NetworkPacket<PlayerControllerMessage>
    {
        public override NetworkMessageIdenfitier Identifier { get; } = NetworkMessageIdenfitier.Player;
        protected override void OnPacketReceived(NetworkUtils.Header header, PlayerControllerMessage body)
        {
            if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(
                    body.NetworkObjectId, out NetworkObject networkObject))
            {
                networkObject.transform.localScale = body.Scale;
            }
        }
    }
}