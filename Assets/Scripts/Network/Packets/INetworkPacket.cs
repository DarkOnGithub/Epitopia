using Unity.Netcode;

namespace Network.Packets
{
    public interface INetworkPacket
    {
        short PacketId { get; }
        void OnPacketReceived(INetworkSerializable packetData);
        int GetSizeOfPacket(INetworkSerializable packetData);
    }
}