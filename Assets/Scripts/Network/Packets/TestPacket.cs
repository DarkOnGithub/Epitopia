using Unity.Netcode;
using UnityEngine;

namespace Network.Packets
{
    
    public struct TextPacket : IPacketData
    {
        public string Text;

    }
    public class TestPacket : NetworkPacket<TextPacket>
    {
        public override PacketType PacketType => PacketType.NetworkPacket;

        public override void OnPacketReceived(TextPacket packetData)
        {
            Debug.Log(packetData.Text);
        }
        public override void SerializePacket(FastBufferWriter writer, TextPacket packet)
        {
            writer.WriteValue(packet.Text);
        }

        public override TextPacket DeserializePacket(FastBufferReader reader)
        {
            TextPacket packet = new TextPacket();
            reader.ReadValueSafe(out packet.Text);
            return packet;
        }
    }
}