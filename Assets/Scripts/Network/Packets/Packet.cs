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
    public interface IPacketData
    {
    }
    public abstract class NetworkPacket<T> where T: IPacketData
    {
        private static Dictionary<PacketType, byte> _packetCount = new();
        private readonly BetterLogger _logger = new(typeof(NetworkPacket<T>));
        private readonly int _size = Marshal.SizeOf<T>();
        public int PacketId;
        public abstract PacketType PacketType { get; }
        public NetworkPacket()
        {
            PacketFactory.RegisterPacket(this);
            if (!_packetCount.TryGetValue(PacketType, out byte value))
                _packetCount[PacketType] = 0;
            
            PacketId = ((byte)PacketType << 8 | _packetCount[PacketType]++);
        }
        
        public abstract void OnPacketReceived(T packetData);
        public abstract void SerializePacket(FastBufferWriter writer, T packetData);
        public abstract T DeserializePacket(FastBufferReader reader);

        public void SendPacket(T packetData)
        {
            
            var size = GetSizeOfPacket(packetData);
            Debug.Log(size);
            PacketFactory.SendPacket(packetData, this, size);
        }

        private IEnumerable<IEnumerable> GetIterableField(T packet)
        {
            FieldInfo[] fields = packet.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            return fields.Where(field => 
                                    field.FieldType == typeof(string) ||
                                    field.FieldType.IsArray ||
                                    (field.FieldType.IsGenericType && 
                                     (field.FieldType.GetGenericTypeDefinition() == typeof(List<>) ||
                                      field.FieldType.GetGenericTypeDefinition() == typeof(Dictionary<,>) ||
                                      field.FieldType.GetGenericTypeDefinition() == typeof(HashSet<>))))
                         .Select(field => (IEnumerable)field.GetValue(packet));
        }
        

        protected int GetSizeOfPacket(T packet)
        {
            int size = _size; 

            foreach (var field in GetIterableField(packet))
            {
                switch (field)
                {
                    case Array array:
                        size += array.Length * Marshal.SizeOf(array.GetType().GetElementType());
                        break;
                    case IList list:
                        size += list.Count * Marshal.SizeOf(list.GetType().GetGenericArguments()[0]);
                        break;
                    case IDictionary dictionary:
                        size += dictionary.Count * (Marshal.SizeOf(dictionary.GetType().GetGenericArguments()[0]) + Marshal.SizeOf(dictionary.GetType().GetGenericArguments()[1]));
                        break;
                    case string str:
                        size += str.Length * sizeof(char);
                        break;
                    default:
                        _logger.LogWarning("Unknown iterable type");
                        break;
                }
            }

            return size;
        }
    }
}