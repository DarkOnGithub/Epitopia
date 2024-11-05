using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Core;
using Events.EventHandler;
using Mono.CSharp;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using Event = UnityEngine.Event;

namespace Network.Packets
{
    public abstract class NetworkPacket<T> : INetworkPacket
        where T : IPacketData
    {
        // protected abstract PacketType PacketType { get; }
        // public short PacketId { get; set; }
        // private int _size = Marshal.SizeOf<T>();
        //
        // public NetworkPacket()
        // {
        //     PacketId = PacketFactory.GetPacketId<T>(PacketType);
        //     PacketFactory.RegisterPacket<T>(this);
        // }
        //
        // protected abstract void OnPacketReceived(T packetData);
        // protected abstract void SerializePacket(FastBufferWriter writer, T packetData);
        // protected abstract T DeserializePacket(FastBufferReader reader);
        //
        // int INetworkPacket.GetSizeOfPacket(INetworkSerializable packet) => GetSizeOfPacket((T)packet);
        //
        // void INetworkPacket.OnPacketReceived(INetworkSerializable packetData)
        // {
        //     OnPacketReceived((T)packetData);
        // }
        //
        // private IEnumerable<IEnumerable> GetIterableField(T packet)
        // {
        //     var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        //     return fields
        //         .Where(field => IsIterableType(field.FieldType))
        //         .Select(field => (IEnumerable)field.GetValue(packet));
        // }
        //
        // private bool IsIterableType(Type type)
        // {
        //     return type == typeof(string)
        //            || type.IsArray
        //            || (type.IsGenericType && IsGenericIEnumerable(type.GetGenericTypeDefinition()));
        // }
        //
        // private bool IsGenericIEnumerable(Type genericType)
        // {
        //     return genericType == typeof(List<>)
        //            || genericType == typeof(Dictionary<,>)
        //            || genericType == typeof(HashSet<>);
        // }
        // int GetSizeOfPacket(T packet)
        // {
        //     int size = _size;
        //     foreach (var field in GetIterableField((T)packet))
        //         size += GetSizeOfField(field);
        //     return size;
        // }
        //
        // private int GetSizeOfField(object field)
        // {
        //     switch (field)
        //     {
        //         case Array array:
        //             return array.Length * Marshal.SizeOf(array.GetType().GetElementType()) + sizeof(int);
        //         case ICollection collection:
        //             return collection.Count * Marshal.SizeOf(collection.GetType().GetGenericArguments()[0]) +
        //                    sizeof(int);
        //         case string str:
        //             return str.Length * sizeof(char) + sizeof(int);
        //         default:
        //             Debug.LogWarning($"Unknown iterable type: {field.GetType()}");
        //             return 0;
        //     }
        // }
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
        protected  abstract PacketType PacketType { get; }
        public short PacketId { get; }
        public int DefaultPacketSize { get; }
        public Type PacketDataType => typeof(T);

        protected NetworkPacket()
        {
            var size = 0;
            foreach (var field in PacketFactory.GetSerializableFields(typeof(T)))
                if(PacketFactory.PrimitivesType.Contains(field.FieldType))
                    size += Marshal.SizeOf(field.FieldType);
            Debug.Log(size);
            DefaultPacketSize = size;
            PacketId = PacketFactory.GeneratePacketId<T>(PacketType);
            PacketFactory.RegisterPacket(this);
        }

        protected abstract void OnPacketReceived(T packetData);
        protected abstract void SerializePacket(FastBufferWriter writer, T packetData);
        protected abstract T DeserializePacket(FastBufferReader reader);

        void INetworkPacket.OnPacketReceived(IPacketData packetData) =>
            OnPacketReceived((T)packetData);

        void INetworkPacket.SerializePacket(FastBufferWriter writer, IPacketData packetData) =>
            SerializePacket(writer, (T)packetData);
        
        IPacketData INetworkPacket.DeserializePacket(FastBufferReader reader) =>
            DeserializePacket(reader);
        private void WriteObjectToBuffer(FastBufferWriter writer, object obj)
        {
            switch (obj)
            {
                case char i:
                    writer.WriteValue(i);
                    break;
                case sbyte i:
                    writer.WriteValue(i);
                    break;
                case byte i:
                    writer.WriteValue(i);
                    break;
                case short i:
                    writer.WriteValue(i);
                    break;
                case ushort i:
                    writer.WriteValue(i);
                    break;
                case int i:
                    writer.WriteValue(i);
                    break;
                case uint i:
                    writer.WriteValue(i);
                    break;
                case long i:
                    writer.WriteValue(i);
                    break;
                case ulong i:
                    writer.WriteValue(i);
                    break;
                case float i:
                    writer.WriteValue(i);
                    break;
                case double i:
                    writer.WriteValue(i);
                    break;
                case bool i:
                    writer.WriteValue(i);
                    break;
                case decimal i:
                    writer.WriteValue(i);
                    break;
                default:
                    if (obj is Array array)
                        SerializeArray(writer, array);
                    else if (obj is IList list)
                        SerializeList(writer, list);
                    else if (obj is IDictionary dictionary)
                        SerializeDictionary(writer, dictionary);
                    else if (obj is string str)
                        SerializeString(writer, str);
                    else
                        Debug.LogWarning($"Unknown type: {obj.GetType()}");
                    break;
            }
        }

        private void ReadObjectFromBuffer<T>(FastBufferReader reader, out object obj)
        {
            var objType = typeof(T);
            if (objType == typeof(char))
            {
                reader.ReadValueSafe(out char c);
                obj = c;
            }
            else if (objType == typeof(sbyte))
            {
                reader.ReadValueSafe(out sbyte c);
                obj = c;
            }
            else if (objType == typeof(byte))
            {
                reader.ReadValueSafe(out byte c);
                obj = c;
            }
            else if (objType == typeof(short))
            {
                reader.ReadValueSafe(out short c);
                obj = c;
            }
            else if (objType == typeof(ushort))
            {
                reader.ReadValueSafe(out ushort c);
                obj = c; }
            else if (objType == typeof(int))
            {
                reader.ReadValueSafe(out int c);
                obj = c;
            }
            else if (objType == typeof(uint))
            {
                reader.ReadValueSafe(out uint c);
                obj = c;
            }
            else if (objType == typeof(long))
            {
                reader.ReadValueSafe(out long c);
                obj = c;
            }
            else if (objType == typeof(ulong))
            {
                reader.ReadValueSafe(out ulong c);
                obj = c;
            }
            else if (objType == typeof(float))
            {
                reader.ReadValueSafe(out float c);
                obj = c; }
            else if (objType == typeof(double))
            {
                reader.ReadValueSafe(out double c);
                obj = c;
            }
            else if (objType == typeof(bool))
            {
                reader.ReadValueSafe(out bool c);
                obj = c;
            }
            else if(objType == typeof(decimal))
            {
                reader.ReadValueSafe(out decimal c);
                obj = c;
            }
            else
            {
                if (objType.IsArray)
                {
                    DeserializeArray(reader, out T[] array);
                    obj = array;
                }
                else if (objType.IsGenericType && objType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    DeserializeList(reader, out List<T> list);
                    obj = list;
                }
                else if (objType.IsGenericType && objType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                {
                    DeserializeDictionary(reader, out Dictionary<T, T> dictionary);
                    obj = dictionary;
                }
                else if (objType == typeof(string))
                {
                    DeserializeString(reader, out string str);
                    obj = str;
                }
                else
                {
                    Debug.LogWarning($"Unknown type: {objType}");
                    obj = null;
                }
            }
        }

        protected void SerializeString(FastBufferWriter writer, string str)
        {
            writer.WriteValue(str.Length);
            foreach (var c in str)
                writer.WriteValue(c);
        }

        protected void SerializeArray(FastBufferWriter writer, Array array)
        {
            writer.WriteValue(PacketFactory.GetSerializableSize(array));
            
            var elementType = array.GetType().GetElementType();
            if (PacketFactory.IsSerializableType(elementType))
                foreach (var item in array)
                    WriteObjectToBuffer(writer, item);
        }
        
        protected void SerializeList(FastBufferWriter writer, IList list)
        {
            writer.WriteValue(PacketFactory.GetSerializableSize(list));
            var elementType = list.GetType().GetGenericArguments()[0];
            if (PacketFactory.IsSerializableType(elementType))
                foreach (var item in list)
                    WriteObjectToBuffer(writer, item);
        }
        
        protected void SerializeDictionary(FastBufferWriter writer, IDictionary dictionary)
        {
            writer.WriteValue(PacketFactory.GetSerializableSize(dictionary));
            var keyType = dictionary.GetType().GetGenericArguments()[0];
            var valueType = dictionary.GetType().GetGenericArguments()[1];
            if (PacketFactory.IsSerializableType(keyType) && PacketFactory.IsSerializableType(valueType))
                foreach (DictionaryEntry entry in dictionary)
                {
                    WriteObjectToBuffer(writer, entry.Key);
                    WriteObjectToBuffer(writer, entry.Value);
                }
        }
        protected void DeserializeString(FastBufferReader reader, out string str)
        {
            reader.ReadValueSafe(out int length);
            str = new string(Enumerable.Range(0, length).Select(_ => { reader.ReadValueSafe(out char c); return c; }).ToArray());
        }
        
        protected void DeserializeArray<TT>(FastBufferReader reader, out TT[] array)
        {
            reader.ReadValueSafe(out int length);
            length /= Marshal.SizeOf<TT>();
            array = new TT[length];
            
            for (int i = 0; i < length; i++)
            {
                ReadObjectFromBuffer<TT>(reader, out var obj);
                array[i] = (TT)obj;
            }
        }
        
        protected void DeserializeList<TT>(FastBufferReader reader, out List<TT> list)
        {
            reader.ReadValueSafe(out int length);
            length /= Marshal.SizeOf<TT>();
            list = new List<TT>(length);
            for (int i = 0; i < length; i++)
            {
                ReadObjectFromBuffer<TT>(reader, out var obj);
                list.Add((TT)obj);
            }
        }
        
        protected void DeserializeDictionary<TKey, TValue>(FastBufferReader reader, out Dictionary<TKey, TValue> dictionary)
        {
            reader.ReadValueSafe(out int length);
            length /= Marshal.SizeOf<TKey>() + Marshal.SizeOf<TValue>();
            dictionary = new Dictionary<TKey, TValue>(length);
            for (int i = 0; i < length; i++)
            {
                ReadObjectFromBuffer<TKey>(reader, out var key);
                ReadObjectFromBuffer<TValue>(reader, out var value);
                dictionary.Add((TKey)key, (TValue)value);
            }
        }
    }
}