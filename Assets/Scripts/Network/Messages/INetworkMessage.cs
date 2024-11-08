using System;
using JetBrains.Annotations;
using Network.Messages.Packets;
using Unity.Netcode;

namespace Network.Messages
{
    public interface INetworkMessage
    {
        Type MessageType { get; }
        NetworkMessageIdenfitier Identifier { get; }

        short PacketId { get; }
        void OnPacketReceived(NetworkUtils.Header header, IMessageData messageData);
        void SendMessageTo(IMessageData messageData, SendingMode mode, ulong author, [CanBeNull] ulong[] clients);

        
    }
}