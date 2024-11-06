using MessagePack;
using UnityEngine;

namespace Network.Packets.Packets.Test
{
    [MessagePackObject]
    public struct MousePacketData : IMessageData
    {
        [Key(0)]
        public float X;
        
        [Key(1)]
        public float Y;
        
        [Key(2)]
        public float Time;
    }
    public class MousePacketTest : NetworkPacket<MousePacketData>
    {
        public override NetworkMessageIdenfitier Identifier { get; } = NetworkMessageIdenfitier.Network;
        protected override void OnPacketReceived(MousePacketData messageData)
        {
            var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f; 
            
            GameObject.Find("Triangle").transform.position = mouseWorldPos;        
        }
    }
}