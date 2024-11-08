using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Core;
using Events.EventHandler;
using JetBrains.Annotations;
using MessagePack;
using Mono.CSharp;
using Network.Messages.Packets;
using Unity.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using Event = UnityEngine.Event;

namespace Network.Messages
{
    public abstract class NetworkPacket<T> : INetworkMessage
        where T : IMessageData
    {
        public BetterLogger Logger { get; } = new(typeof(NetworkPacket<T>));
        public bool IsHost => NetworkManager.Singleton.IsHost;
        public abstract NetworkMessageIdenfitier Identifier { get; }
        public Type MessageType { get; }
        public int PacketId { get; }

        public NetworkPacket()
        {
            MessageType = typeof(T);
            PacketId = MessageFactory.GeneratePacketId(Identifier);
            MessageFactory.RegisterPacket(this);
        }

        protected abstract void OnPacketReceived(NetworkUtils.Header header, T body);

        void INetworkMessage.OnPacketReceived(NetworkUtils.Header header, IMessageData body)
        {
            OnPacketReceived(header, (T)body);
        }

        void INetworkMessage.SendMessageTo(IMessageData messageData, SendingMode mode, ulong author,
            [CanBeNull] ulong[] clients, NetworkDelivery delivery)
        {
            var message = MessagePackSerializer.Serialize<T>((T)messageData);
            var header = NetworkUtils.GenerateHeader(mode, PacketId, author, clients);

            var size = sizeof(int) + header.Length + sizeof(int) + message.Length;

            if (MessageFactory.IsInitialized)
            {
                var writer = new FastBufferWriter(size, Allocator.Temp);
                if (!writer.TryBeginWrite(size)) return;
                writer.WriteValue(header);
                writer.WriteValue(message.Length);
                writer.WriteBytes(message);
                MessageFactory.SendBufferTo(writer, mode, delivery, clients );
            }
        }
    }
}