using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Core;
using Events.EventHandler;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

namespace Network.Packets
{


    public abstract class NetworkPacket<T> : INetworkPacket where T : INetworkSerializable
    {
        protected abstract PacketType PacketType { get; }
        public short PacketId { get; set; }
        private int _size = Marshal.SizeOf<T>();

        public NetworkPacket()
        {
            PacketId = PacketFactory.GetPacketId<T>(PacketType);
            PacketFactory.RegisterPacket<T>(this);
        }

        protected abstract void OnPacketReceived(T packetData);

        void INetworkPacket.OnPacketReceived(INetworkSerializable packetData)
        {
            OnPacketReceived((T)packetData);
        }

        private IEnumerable<IEnumerable> GetIterableField(T packet)
        {
            var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            return fields
                .Where(field => IsIterableType(field.FieldType))
                .Select(field => (IEnumerable)field.GetValue(packet));
        }

        private bool IsIterableType(Type type)
        {
            return type == typeof(string)
                   || type.IsArray
                   || (type.IsGenericType && IsGenericIEnumerable(type.GetGenericTypeDefinition()));
        }

        private bool IsGenericIEnumerable(Type genericType)
        {
            return genericType == typeof(List<>)
                   || genericType == typeof(Dictionary<,>)
                   || genericType == typeof(HashSet<>);
        }
        int GetSizeOfPacket(T packet)
        {
            int size = _size;

            foreach (var field in GetIterableField((T)packet))
            {
                size += GetSizeOfField(field);
            }

            return size;
        }
        int INetworkPacket.GetSizeOfPacket(INetworkSerializable packet)
        {
            return GetSizeOfPacket((T)packet);
        }

        private int GetSizeOfField(object field)
        {
            switch (field)
            {
                case Array array:
                    return array.Length * Marshal.SizeOf(array.GetType().GetElementType()) + sizeof(int);
                case ICollection collection:
                    return collection.Count * Marshal.SizeOf(collection.GetType().GetGenericArguments()[0]) +
                           sizeof(int);
                case string str:
                    return str.Length * sizeof(char) + sizeof(int);
                default:
                    Debug.LogWarning($"Unknown iterable type: {field.GetType()}");
                    return 0;
            }
        }
        // public interface IPacketData : INetworkSerializable
        // {
        // }
        //
        // public interface INetworkPacket
        // {
        //     int PacketId { get; set; }
        //     void SerializePacket(FastBufferWriter writer, IPacketData packetData);
        //     IPacketData DeserializePacket(FastBufferReader reader);
        //     void OnPacketReceived(IPacketData packetData);
        //     void SendPacket(IPacketData packetData);
        // }
        //
        // public abstract class NetworkPacket<T> : INetworkPacket where T : IPacketData
        // {
        //     private static Dictionary<PacketType, byte> _packetCount = new();
        //     private readonly BetterLogger _logger = new(typeof(NetworkPacket<T>));
        //     private readonly int _size = Marshal.SizeOf<T>();
        //     public int PacketId { get; set; }
        //     public abstract PacketType PacketType { get; }
        //
        //     public NetworkPacket()
        //     {
        //         SetPacketId();
        //         PacketFactory.RegisterPacket<T>(this);
        //     }
        //
        //     public abstract void OnPacketReceived(T packetData);
        //     public abstract void SerializePacket(FastBufferWriter writer, T packetData);
        //     public abstract T DeserializePacket(FastBufferReader reader);
        //
        //     void INetworkPacket.OnPacketReceived(IPacketData packetData)
        //     {
        //         OnPacketReceived((T)packetData);
        //     }
        //
        //     void INetworkPacket.SerializePacket(FastBufferWriter writer, IPacketData packetData)
        //     {
        //         SerializePacket(writer, (T)packetData);
        //     }
        //
        //     IPacketData INetworkPacket.DeserializePacket(FastBufferReader reader)
        //     {
        //         return DeserializePacket(reader);
        //     }
        //
        //     void INetworkPacket.SendPacket(IPacketData packetData)
        //     {
        //         SendPacket((T)packetData);
        //     }
        //
        //     private void SetPacketId()
        //     {
        //         if (!_packetCount.TryGetValue(PacketType, out var value))
        //             _packetCount[PacketType] = 0;
        //         PacketId = ((byte)PacketType << 8) | _packetCount[PacketType]++;
        //     }
        //
        //     public void SendPacket(T packetData)
        //     {
        //         var size = GetSizeOfPacket(packetData);
        //         PacketFactory.SendPacket(packetData, this, size);
        //     }
        //
        //     private IEnumerable<IEnumerable> GetIterableField(T packet)
        //     {
        //         var fields = packet.GetType()
        //             .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        //         return fields.Where(field =>
        //                 field.FieldType == typeof(string) ||
        //                 field.FieldType.IsArray ||
        //                 (field.FieldType.IsGenericType &&
        //                  (field.FieldType.GetGenericTypeDefinition() == typeof(List<>) ||
        //                   field.FieldType.GetGenericTypeDefinition() == typeof(Dictionary<,>) ||
        //                   field.FieldType.GetGenericTypeDefinition() == typeof(HashSet<>)))
        //                 )
        //             .Select(field => (IEnumerable)field.GetValue(packet));
        //     }
        //
        //     protected int GetSizeOfPacket(T packet)
        //     {
        //         var size = _size;
        //
        //         foreach (var field in GetIterableField(packet))
        //             switch (field)
        //             {
        //                 case Array array:
        //                     size += array.Length * Marshal.SizeOf(array.GetType().GetElementType()) + sizeof(int);
        //                     break;
        //                 case IList list:
        //                     size += list.Count * Marshal.SizeOf(list.GetType().GetGenericArguments()[0]) + sizeof(int);
        //                     break;
        //                 // case IDictionary dictionary:
        //                 //     size += dictionary.Count * (Marshal.SizeOf(dictionary.GetType().GetGenericArguments()[0]) +
        //                 //                                 Marshal.SizeOf(dictionary.GetType().GetGenericArguments()[1]));
        //                 //     break;
        //                 case string str:
        //                     size += str.Length * sizeof(char) + sizeof(int);
        //                     break;
        //                 default:
        //                     _logger.LogWarning("Unknown iterable type");
        //                     break;
        //             }
        //
        //         return size;
        //     }
        //     
        //     public void SerializeString(FastBufferWriter writer, string str)
        //     {
        //         writer.WriteValue(str.Length);
        //         foreach (var c in str)
        //             writer.WriteValue(c);
        //     }
        //     
        //     public void SerializeArray<T>(FastBufferWriter writer, T[] array)
        //     {
        //         writer.WriteValue(array.Length);
        //         foreach (var item in array)
        //             writer.WriteValue(item);
        //     }
        // }
    }
}