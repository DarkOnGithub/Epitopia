// using System;
// using Unity.Netcode;
// using UnityEngine;
//
// namespace Network.Packets
// {
//     public struct TextPacket : IPacketData
//     {
//         public string Text;
//         public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
//         {
//             serializer.SerializeValue(ref Text);
//         }
//     }
//
//     public class TestPacket : NetworkPacket<TextPacket>
//     {
//         public override PacketType PacketType { get; }
//
//         public override void OnPacketReceived(TextPacket packetData)
//         {
//             Debug.Log(packetData.Text);
//         }
//
//         public override void SerializePacket(FastBufferWriter writer, TextPacket packetData)
//         {
//             writer.WriteValue(packetData.Text);
//         }
//
//         public override TextPacket DeserializePacket(FastBufferReader reader)
//         {
//             var packet = new TextPacket();
//             reader.ReadValue(out packet.Text);
//             return packet;
//         }
//     }
// }