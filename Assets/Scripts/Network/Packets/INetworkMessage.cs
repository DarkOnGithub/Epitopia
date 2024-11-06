using System;
using Unity.Netcode;

namespace Network.Packets
{
    public interface INetworkMessage
    {
        Type MessageType { get; }
        NetworkMessageIdenfitier Identifier { get; }

        short PacketId { get; }
        void OnPacketReceived(IMessageData messageData);
        void SendMessage(IMessageData messageData, ulong[] clients);
    }
}