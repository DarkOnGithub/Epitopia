using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Netcode;
using Unity.Services.Relay.Models;
using UnityEngine;

namespace Network.Packets
{
    public static class PacketFactory
    {
        private static Dictionary<Type, INetworkPacket> _packetDataToPacket = new();
        private static Dictionary<int, INetworkPacket> _idToPacket = new();
        private static Dictionary<Type, byte> _packetCount = new();
        public static void Initialize()
        {
            
        }
        
        public static void Terminate()
        {
            
        }
        public static short GetPacketId<T>(PacketType packetType) where T : INetworkSerializable
        {
            if(!_packetCount.ContainsKey(typeof(T)))
                _packetCount[typeof(T)] = 0;
            return (byte)((byte)packetType << 8 | _packetCount[typeof(T)]++);
        }
        
        public static void RegisterPacket<T>(INetworkPacket packet) where T : INetworkSerializable
        {
            _packetDataToPacket[typeof(T)] = packet;
            _idToPacket[packet.PacketId] = packet;
        }
        
        public static void SendPacket<T>(T packetData) where T : INetworkSerializable
        {
            
        }

        public static FastBufferWriter SerializePacket(INetworkSerializable packetData)
        { 
            var writer = new FastBufferWriter(_packetDataToPacket[packetData.GetType()], Allocator.Temp);
          throw new NotImplementedException();
        }
        
        public static void DeserializePacket(INetworkSerializable packetData)
        {
            
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