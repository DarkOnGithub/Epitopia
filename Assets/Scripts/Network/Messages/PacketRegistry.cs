using Network.Messages.Packets.Network;
using Network.Messages.Packets.Players;
using Network.Messages.Packets.World;

namespace Network.Messages
{
    public static class PacketRegistry
    {
        public static void RegisterPackets()
        {
            new ConnectionPacket();
            new ChunkRequestPacket();
            new ChunkTransferPacket();
            new PlayerControllerPacket();
            new BlockActionPacket();
        }
    }
}