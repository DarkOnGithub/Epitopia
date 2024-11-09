using Network.Messages.Packets.Network;
using Network.Messages.Packets.World;
using UnityEngine;

namespace Network.Messages
{
    public static class PacketRegistry
    {
        public static void RegisterPackets()
        {
            new ConnectionPacket();
            new PlayerChunkRequest();
            new ChunkReceiver();

        }
    }
}