using MessagePack;
using UnityEngine;

namespace Network.Messages.Packets.Network
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
        protected override void OnPacketReceived(NetworkUtils.Header header, HandShakeData body)
        {
            Debug.Log("User " + body.ClientId + " has connected");
        }
    }
}