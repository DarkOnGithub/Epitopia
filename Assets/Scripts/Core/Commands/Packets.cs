using System.Collections.Generic;
using MessagePack;
using Network.Packets;
using QFSW.QC;
using Unity.Netcode;
using UnityEngine;

namespace Core.Commands
{
    public class Packets
    {
       
        [Command]
        public static void SendPacket()
        {
            var packet = new TestMessageData() { ETA = Time.realtimeSinceStartup * 1000, SoloQ = new double[1] };
            Debug.Log(packet);
            PacketFactory.SendPacketToAll(packet);
        }
    }
}