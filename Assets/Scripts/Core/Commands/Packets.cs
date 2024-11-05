using System.Collections.Generic;
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
            var packet = new TestPacketData() {ETA = Time.realtimeSinceStartup * 1000, SoloQ = new double[1]};
            PacketFactory.SendPacketToAll(packet);
            
        }
    }
}