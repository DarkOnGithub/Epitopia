using MessagePack;
using UnityEngine;

namespace Network.Packets.Packets.Network
{
    [MessagePackObject]
    public struct HandShakeData : IMessageData
    {
        [Key(0)]
        public ulong ClientId { get; set; }
    }
    public class HandShake : NetworkPacket<HandShakeData>
    {
        public override NetworkMessageIdenfitier Identifier { get; } = NetworkMessageIdenfitier.Network;
        protected override void OnPacketReceived(HandShakeData messageData)
        {
            Debug.Log("User" + messageData.ClientId + " has connected");
        }
    }
}