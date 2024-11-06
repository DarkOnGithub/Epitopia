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
using Unity.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using Event = UnityEngine.Event;

namespace Network.Packets
{
    public abstract class NetworkPacket<T> : INetworkMessage
        where T : IMessageData
    {
        public abstract NetworkMessageIdenfitier Identifier { get; }
        public Type MessageType { get; }
        public short PacketId { get; }

        public NetworkPacket()
        {
            MessageType = typeof(T);
            PacketId = MessageFactory.GeneratePacketId(Identifier);
            MessageFactory.RegisterPacket(this);
        }
        
        protected abstract void OnPacketReceived(T messageData);
        void INetworkMessage.OnPacketReceived(IMessageData messageData)
        {
            OnPacketReceived((T)messageData);
        }
        
        void INetworkMessage.SendMessageToServer(IMessageData messageData, [CanBeNull] ulong[] clients)
        {
            
        }
        
        void INetworkMessage.SendMessageToClients(IMessageData messageData, [CanBeNull] ulong[] clients)
        {
            byte[] message = MessagePackSerializer.Serialize<T>((T)messageData);
            var size = sizeof(byte) + sizeof(short) + sizeof(int) + message.Length;
            if (MessageFactory.IsInitialized)
            {
                using (var writer = new FastBufferWriter(size, Allocator.Temp))
                {
                    if (!writer.TryBeginWrite(size)) return;
                    byte header = 0b0;
                    if (clients != null && clients.Length > 0)
                        header = (byte)(clients.Length & 0x7F | 0x80);
                    
                    writer.WriteByte(header);
                    writer.WriteValue(PacketId);
                    writer.WriteValue(message.Length);
                    writer.WriteBytes(message);
                    MessageFactory.SendBufferTo(writer, clients);
                }
            }
        }
    }
}