using System;
using Core;
using JetBrains.Annotations;
using MessagePack;
using Unity.Collections;
using Unity.Netcode;

namespace Network.Messages
{
    public enum PacketSource
    {
        Server,
        Client
    }

    public abstract class NetworkPacket<T> : INetworkMessage
        where T : IMessageData
    {
        public NetworkPacket()
        {
            MessageType = typeof(T);
            PacketId = MessageFactory.GeneratePacketId(Identifier);
            MessageFactory.RegisterPacket(this);
        }

        public BetterLogger Logger { get; } = new(typeof(NetworkPacket<T>));
        public bool IsHost => NetworkManager.Singleton.IsHost;
        public abstract NetworkMessageIdenfitier Identifier { get; }
        public Type MessageType { get; }
        public int PacketId { get; }

        void INetworkMessage.OnPacketReceived(NetworkUtils.Header header, IMessageData body)
        {
            OnPacketReceived(header, (T)body);
        }

        void INetworkMessage.SendMessageTo(IMessageData messageData, SendingMode mode, ulong author,
            [CanBeNull] ulong[] clients, NetworkDelivery delivery)
        {
            var message = MessagePackSerializer.Serialize((T)messageData);
            var header = NetworkUtils.GenerateHeader(mode, PacketId, author, clients);

            var size = sizeof(int) + header.Length + sizeof(int) + message.Length;
            if (MessageFactory.IsInitialized)
            {
                var writer = new FastBufferWriter(size, Allocator.Temp);
                if (!writer.TryBeginWrite(size)) return;
                writer.WriteValue(header);
                writer.WriteValue(message.Length);
                writer.WriteBytes(message);
                MessageFactory.SendBufferTo(writer, mode, delivery, clients);
            }
        }

        protected abstract void OnPacketReceived(NetworkUtils.Header header, T body);
    }
}