using System;
using JetBrains.Annotations;
using Unity.Netcode;

namespace Network.Packets
{
    public interface INetworkMessage
    {
        Type MessageType { get; }
        NetworkMessageIdenfitier Identifier { get; }

        short PacketId { get; }
        void OnPacketReceived(IMessageData messageData);
        void SendMessageToClients(IMessageData messageData, [CanBeNull] ulong[] clients);

        void SendMessageToServer(IMessageData messageData, [CanBeNull] ulong[] clients);
        
    }
}