using System;
using System.Collections.Generic;
using MessagePack;
using QFSW.QC;
using Unity.Netcode;
using UnityEngine;

namespace Network.Packets
{
    [MessagePackObject]
    public struct TestMessageData : IMessageData
    {
        [Key(0)]
        public float ETA { get; set; }
        [Key(1)]
        public double[] SoloQ { get; set; }

        public override string ToString()
        {
            return ETA.ToString();
        }
    }

    public class TestPacket : NetworkPacket<TestMessageData>
    {
        public override NetworkMessageIdenfitier Identifier { get; } = NetworkMessageIdenfitier.Network;
        protected override void OnPacketReceived(TestMessageData messageData)
        {
            Debug.Log(messageData);
            Debug.Log(messageData.ETA);
        }
    }
}