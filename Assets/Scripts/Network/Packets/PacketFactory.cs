using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Core;
using Unity.Collections;
using Unity.Netcode;
using Unity.Services.Relay.Models;
using Debug = UnityEngine.Debug;
using Type = System.Type;

namespace Network.Packets
{
    public static class PacketFactory
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
        public static readonly Dictionary<short, INetworkPacket> PacketIds = new();
        public static readonly Dictionary<Type, INetworkPacket> PacketTypes = new();
        private static CustomMessagingManager _messagingManager;
        private static readonly BetterLogger Logger = new(typeof(PacketFactory));

        
        
        
        public static void RegisterAllPackets()
        {
            new TestPacket();
        }
        
        
        
        public static void Initialize()
        {
            _messagingManager = NetworkManager.Singleton.CustomMessagingManager;
            _messagingManager.OnUnnamedMessage += OnUnnamedMessageReceived;
        }

        public static void Dispose()
        {
            _messagingManager.OnUnnamedMessage -= OnUnnamedMessageReceived;
            _messagingManager = null;
        }
        public static bool IsSerializableType(object obj)
        {
            if (PrimitivesType.Contains(obj))
                return true;
            return obj is Array or IDictionary or IList;
        }
        public static bool IsSerializableType(FieldInfo obj)
        {
            if (PrimitivesType.Contains(obj.FieldType))
                return true;
            return obj.FieldType == typeof(string) || obj.FieldType.IsArray || obj.FieldType.GetInterface(nameof(IDictionary)) != null || obj.FieldType.GetInterface(nameof(IList)) != null;
        }

        public static int GetSerializableSize(object obj)
        {
            if(PrimitivesType.Contains(obj.GetType()))
                return Marshal.SizeOf(obj);
            
            
            var size = 0;
            
            if(obj is Array array)
                foreach (var element in array)
                    size += GetSerializableSize(element);
            
            else if(obj is IDictionary dictionary)
                foreach (DictionaryEntry entry in dictionary)
                    size += GetSerializableSize(entry.Key) + GetSerializableSize(entry.Value);
            
            else if(obj is IList list)
                foreach (var element in list)
                    size += GetSerializableSize(element);
            
            else if(obj is string str)
                size += str.Length * sizeof(char);
            return size;

        }
        private static void OnUnnamedMessageReceived(ulong clientId, FastBufferReader reader)
        {  
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            reader.ReadValueSafe(out short packetId);
            if (!PacketIds.TryGetValue(packetId, out var packet))
            {
                Logger.LogWarning($"Received unknown packet with id {packetId.ToHex()}");
                return;
            }
            var packetData = packet.DeserializePacket(reader);
            packet.OnPacketReceived(packetData);
            stopwatch.Stop();
            Console.WriteLine($"Execution Time: {stopwatch.ElapsedMilliseconds} ms");
           
        }
        public static short GeneratePacketId<T>(PacketType packetType) where T: IPacketData
        {
            if (!_packetCount.ContainsKey(typeof(T)))
                _packetCount[typeof(T)] = 0;
            return (short)((byte)packetType << 8 | _packetCount[typeof(T)]++);
        }
        
        public static void RegisterPacket(INetworkPacket packet)
        {
            PacketIds[packet.PacketId] = packet;
            PacketTypes[packet.PacketDataType] = packet;
        }

        private static int SizeOfPacket(INetworkPacket packet, IPacketData packetData)
        {
            var size = packet.DefaultPacketSize;
            foreach (var field in GetSerializableFields(packetData.GetType()))
            {
                if(!IsSerializableType(field))
                    continue;
                size += GetSerializableSize(field.GetValue(packetData));
            }
            return size;
        }
        private static bool WritePacketToBuffer(INetworkPacket packet, IPacketData packetData, FastBufferWriter buffer)
        {
            if (buffer.TryBeginWrite(buffer.Capacity))
            {
                buffer.WriteValue(packet.PacketId);
                packet.SerializePacket(buffer, packetData);
                return true;
            }

            return false;
        }
        public static FastBufferWriter? PreparePacket<T>(T packetData) where T : IPacketData
        {

            if (!PacketTypes.TryGetValue(packetData.GetType(), out var packet))
            {
                Logger.LogWarning($"Failed to send packet {packetData.GetType().Name} as it is not registered");
                return null;
            }
            var size = SizeOfPacket(packet, packetData) + sizeof(short);
            var buffer = new FastBufferWriter(size, Allocator.Temp);

            if(WritePacketToBuffer(packet, packetData, buffer))
                return buffer;

            buffer.Dispose();
            Logger.LogWarning($"Failed to write packet {packetData.GetType().Name} to buffer");
            return null;
        }
        
        public static void SendBufferToAll(FastBufferWriter buffer)
        {
            _messagingManager.SendUnnamedMessageToAll(buffer, NetworkDelivery.ReliableFragmentedSequenced);
            buffer.Dispose();
        }

        public static void SendPacketToAll<T>(T packetData) where T : IPacketData
        {
            var buffer = PreparePacket(packetData);
            if(buffer == null)
                return;
            SendBufferToAll(buffer.Value);
        }
        public static IEnumerable<FieldInfo> GetSerializableFields(Type T)
        {
            return T
                  .GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                  .Where(field => IsSerializableType(field));
        } 
        
        public static string ToHex(this short packetId)
        {
            return "0x" + packetId.ToString("X");
        }
        

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