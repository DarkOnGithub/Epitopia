using System.Collections;
using System.Collections.Generic;
using MessagePack;
using Network;
using Network.Packets;
using Network.Packets.Packets.Test;
using QFSW.QC;
using Tests;
using Unity.Netcode;
using UnityEngine;

namespace Core.Commands
{
    public class Packets
    {
       
        [Command]
        public static void SendPacket()
        {
            TriangleTest.Instance.T();
           
        
        }
    }
}