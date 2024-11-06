using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Core;
using MessagePack;
using Network.Packets.Packets.Network;
using Network.Packets.Packets.Test;
using Unity.Collections;
using Unity.Netcode;
using Unity.Services.Relay.Models;
using Debug = UnityEngine.Debug;
using Type = System.Type;

namespace Network.Packets
{
    public static class MessageFactory
    {

        public static readonly HashSet<Type> PrimitivesType = new()
          {
              typeof(bool),
              typeof(byte),
              typeof(sbyte),
              typeof(char),
              typeof(short),
              typeof(ushort),
              typeof(int),
              typeof(uint),
              typeof(long),
              typeof(ulong),
              typeof(float),
              typeof(double),
              typeof(decimal),
          };

        private static Dictionary<Type, byte> _packetCount = new();
        public static readonly Dictionary<short, INetworkMessage> NetworkMessageIds = new();
        public static readonly Dictionary<Type, INetworkMessage> NetworkMessageTypes = new();
        public  static CustomMessagingManager MessagingManager;
        private static readonly BetterLogger Logger = new(typeof(MessageFactory));

        public static bool IsInitialized => MessagingManager != null;
        
        
        public static void RegisterAllPackets()
        {
            new MousePacketTest();
            new HandShake();
        }
        
        
        
        public static void Initialize()
        {
            MessagingManager = NetworkManager.Singleton.CustomMessagingManager;
            MessagingManager.OnUnnamedMessage += OnUnnamedMessageReceived;

        }

        public static void Dispose()
        {
            MessagingManager.OnUnnamedMessage -= OnUnnamedMessageReceived;
            MessagingManager = null;
        }

        public static void RegisterPacket(INetworkMessage message)
        {
            Logger.LogInfo($"Registering packet {message.GetType().Name} with id {message.PacketId.ToHex()}");
            NetworkMessageIds[message.PacketId] = message;
            NetworkMessageTypes[message.MessageType] = message;
        }
        
        private static void OnUnnamedMessageReceived(ulong clientId, FastBufferReader reader)
        {
            reader.ReadValueSafe(out short packetId);
            if (!NetworkMessageIds.TryGetValue(packetId, out var networkMessage))
            {
                Logger.LogWarning($"Received unknown packet with id {packetId.ToHex()}");
                return;
            }
            
            reader.ReadValueSafe(out int bufferSize);
            if(!reader.TryBeginRead(bufferSize))
                return;
            var buffer = new byte[bufferSize];
            reader.ReadBytes(ref buffer, bufferSize);
            networkMessage.OnPacketReceived((IMessageData)MessagePackSerializer.Deserialize(networkMessage.MessageType, buffer));
        }
        
        public static void SendPacketToAll<T>(T packetData) where T : IMessageData
        {
            if (!NetworkMessageTypes.TryGetValue(packetData.GetType(), out var networkMessage))
            {
                Logger.LogWarning($"Failed to send packet {packetData.GetType().Name} as it is not registered");
                return;
            }
            networkMessage.SendMessage(packetData, null);
        }
        public static short GeneratePacketId(NetworkMessageIdenfitier idenfitier) 
        {
            if (!_packetCount.ContainsKey(idenfitier.GetType()))
                _packetCount[idenfitier.GetType()] = 0;
            return (short)((byte)idenfitier << 8 | _packetCount[idenfitier.GetType()]++);
        }
        public static string ToHex(this short packetId)
        {
            return "0x" + packetId.ToString("X");
        }
        
        // public static bool IsSerializableType(object obj)
        // {
        //     if (PrimitivesType.Contains(obj))
        //         return true;
        //     return obj is Array or IDictionary or IList;
        // }
        // public static bool IsSerializableType(FieldInfo obj)
        // {
        //     if (PrimitivesType.Contains(obj.FieldType))
        //         return true;
        //     return obj.FieldType == typeof(string) || obj.FieldType.IsArray || obj.FieldType.GetInterface(nameof(IDictionary)) != null || obj.FieldType.GetInterface(nameof(IList)) != null;
        // }
        //
        // public static int GetSerializableSize(object obj)
        // {
        //     if(PrimitivesType.Contains(obj.GetType()))
        //         return Marshal.SizeOf(obj);
        //     
        //     
        //     var size = 0;
        //     
        //     if(obj is Array array)
        //         foreach (var element in array)
        //             size += GetSerializableSize(element);
        //     
        //     else if(obj is IDictionary dictionary)
        //         foreach (DictionaryEntry entry in dictionary)
        //             size += GetSerializableSize(entry.Key) + GetSerializableSize(entry.Value);
        //     
        //     else if(obj is IList list)
        //         foreach (var element in list)
        //             size += GetSerializableSize(element);
        //     
        //     else if(obj is string str)
        //         size += str.Length * sizeof(char);
        //     return size;
        //
        // }
        // private static void OnUnnamedMessageReceived(ulong clientId, FastBufferReader reader)
        // {  
        //     Stopwatch stopwatch = new Stopwatch();
        //     stopwatch.Start();
        //
        //     reader.ReadValueSafe(out short packetId);
        //     if (!PacketIds.TryGetValue(packetId, out var packet))
        //     {
        //         Logger.LogWarning($"Received unknown packet with id {packetId.ToHex()}");
        //         return;
        //     }
        //     var packetData = packet.DeserializePacket(reader);
        //     packet.OnPacketReceived(packetData);
        //     stopwatch.Stop();
        //     Console.WriteLine($"Execution Time: {stopwatch.ElapsedMilliseconds} ms");
        //    
        // }
        // public static short GeneratePacketId<T>(Network networkMessageType) where T: IMessageData
        // {
        //     if (!_packetCount.ContainsKey(typeof(T)))
        //         _packetCount[typeof(T)] = 0;
        //     return (short)((byte)networkMessageType << 8 | _packetCount[typeof(T)]++);
        // }
        //
        // public static void RegisterPacket(INetworkMessage message)
        // {
        //     PacketIds[message.PacketId] = message;
        //     PacketTypes[message.PacketDataType] = message;
        // }
        //
        // private static int SizeOfPacket(INetworkMessage message, IMessageData messageData)
        // {
        //     var size = message.DefaultPacketSize;
        //     foreach (var field in GetSerializableFields(messageData.GetType()))
        //     {
        //         if(!IsSerializableType(field))
        //             continue;
        //         size += GetSerializableSize(field.GetValue(messageData));
        //     }
        //     return size;
        // }
        // private static bool WritePacketToBuffer(INetworkMessage message, IMessageData messageData, FastBufferWriter buffer)
        // {
        //     if (buffer.TryBeginWrite(buffer.Capacity))
        //     {
        //         buffer.WriteValue(message.PacketId);
        //         message.SerializePacket(buffer, messageData);
        //         return true;
        //     }
        //
        //     return false;
        // }
        // public static FastBufferWriter? PreparePacket<T>(T packetData) where T : IMessageData
        // {
        //
        //     if (!PacketTypes.TryGetValue(packetData.GetType(), out var packet))
        //     {
        //         Logger.LogWarning($"Failed to send packet {packetData.GetType().Name} as it is not registered");
        //         return null;
        //     }
        //     var size = SizeOfPacket(packet, packetData) + sizeof(short);
        //     var buffer = new FastBufferWriter(size, Allocator.Temp);
        //
        //     if(WritePacketToBuffer(packet, packetData, buffer))
        //         return buffer;
        //
        //     buffer.Dispose();
        //     Logger.LogWarning($"Failed to write packet {packetData.GetType().Name} to buffer");
        //     return null;
        // }
        //
        // public static void SendBufferToAll(FastBufferWriter buffer)
        // {
        //     _messagingManager.SendUnnamedMessageToAll(buffer, NetworkDelivery.ReliableFragmentedSequenced);
        //     buffer.Dispose();
        // }
        //
        // public static void SendPacketToAll<T>(T packetData) where T : IMessageData
        // {
        //     var buffer = PreparePacket(packetData);
        //     if(buffer == null)
        //         return;
        //     SendBufferToAll(buffer.Value);
        // }
        // public static IEnumerable<FieldInfo> GetSerializableFields(Type T)
        // {
        //     return T
        //           .GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
        //           .Where(field => IsSerializableType(field));
        // } 
        //     private static Dictionary<Type, INetworkPacket> PacketDataToPacket = new();
        //     private static Dictionary<int, INetworkPacket> IdToPacket = new();
        //
        //     public static CustomMessagingManager MessagingManager;
        //
        //     public static void Initialize()
        //     {
        //         MessagingManager = NetworkManager.Singleton.CustomMessagingManager;
        //         MessagingManager.OnUnnamedMessage += OnMessageReceived;
        //     }
        //
        //     public static string PacketIdToHex(short packetId)
        //     {
        //         return "0x" + packetId.ToString("X");
        //     }
        //
        //     public static void RegisterPacket<T>(INetworkPacket packet) where T : IPacketData
        //     {
        //         PacketDataToPacket[typeof(T)] = packet;
        //         IdToPacket[packet.PacketId] = packet;
        //     }
        //
        //     private static void OnMessageReceived(ulong clientId, FastBufferReader reader)
        //     {
        //         reader.ReadValueSafe(out short packetId);
        //         if (!IdToPacket.TryGetValue(packetId, out var packet))
        //             return;
        //         var packetData = packet.DeserializePacket(reader);
        //         packet.OnPacketReceived(packetData);
        //     }
        //
        //     public static void SendPacket(IPacketData packetData)
        //     {
        //         if (PacketDataToPacket.TryGetValue(packetData.GetType(), out var packet))
        //             packet.SendPacket(packetData);
        //     }
        //
        //     public static void SendPacket<T, TT>(T packetData, TT packet, int size)
        //         where T : IPacketData
        //         where TT : INetworkPacket
        //     {
        //         using (var writer = new FastBufferWriter(2 + size, Allocator.Temp))
        //         {
        //             if (writer.TryBeginWrite(2 + size))
        //             {
        //                 writer.WriteValue(packet.PacketId);
        //                 packet.SerializePacket(writer, packetData);
        //                 MessagingManager.SendUnnamedMessageToAll(writer);
        //             }
        //         }
        //     }
        // }
    }
}