using System;
using System.Collections.Generic;
using QFSW.QC;
using Unity.Netcode;
using UnityEngine;

namespace Network.Packets
{
    public struct TestPacketData : IPacketData
    {
        public float ETA;
        public double[] SoloQ;
    }

    public class TestPacket : NetworkPacket<TestPacketData>
    {
        protected override PacketType PacketType { get; } = PacketType.NetworkPacket;
        protected override void OnPacketReceived(TestPacketData packetData)
        {
            Debug.Log($"Received packet with ETA: {Time.realtimeSinceStartup * 1000 - packetData.ETA} ms");            Debug.Log(packetData.SoloQ.Length);
        }

        protected override void SerializePacket(FastBufferWriter writer, TestPacketData packetData)
        {
            writer.WriteValue(packetData.ETA);
        }
        
        //!TODO Call readValue instead of readValueSafe and Get the size from default size
        protected override TestPacketData DeserializePacket(FastBufferReader reader)
        {
            TestPacketData packetData = new TestPacketData();
            reader.ReadValueSafe(out packetData.ETA);
            return packetData;        
        }
    }
}