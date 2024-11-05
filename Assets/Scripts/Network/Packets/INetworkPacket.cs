using System;
using Unity.Netcode;

namespace Network.Packets
{
    public interface INetworkPacket
    {
        Type PacketDataType { get; }
        short PacketId { get; }
        int DefaultPacketSize { get; }
        void OnPacketReceived(IPacketData packetData);
        void SerializePacket(FastBufferWriter writer, IPacketData packetData);
        IPacketData DeserializePacket(FastBufferReader reader);
    }
}