using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using Unity.Services.Relay.Models;
using UnityEngine;

namespace Network.Packets
{
    public static class PacketFactory
    {
        public static Dictionary<Type, object> PacketDataToPacket = new();
        public static Dictionary<int, NetworkPacket<IPacketData>> PacketsId = new();

        public static CustomMessagingManager MessagingManager;
        public static void Initialize()
        {
            MessagingManager = NetworkManager.Singleton.CustomMessagingManager;
            Debug.Log(NetworkManager.Singleton);
            Debug.Log(MessagingManager);
            MessagingManager.OnUnnamedMessage += OnMessageReceived;
        }
        
        public static void RegisterPacket<T>(NetworkPacket<T> packet)
            where T: IPacketData
        {
            Debug.Log(packet);
            PacketDataToPacket[typeof(T)] = packet;
            PacketsId[packet.PacketId] = packet as NetworkPacket<IPacketData>;
        }

        private static void OnMessageReceived(ulong clientId, FastBufferReader reader)
        {
            Debug.Log("a");
            reader.ReadValueSafe(out int packetId);
            Debug.Log(packetId);
        }

        public static void SendPacket<T>(T packetData) where T : IPacketData
        {
            if(PacketDataToPacket.TryGetValue(packetData.GetType(), out var packet))
                (packet as NetworkPacket<T>)?.SendPacket(packetData);
        }   
        
        public static void SendPacket<T, TT>(T packetData, TT packet, int size) 
            where T : IPacketData
            where TT : NetworkPacket<T>
        {
            using (var writer = new FastBufferWriter(2 + size, Allocator.Temp))
            {
                if (writer.TryBeginWrite(2 + size))
                {
                    writer.WriteValue(packet.PacketId);
                    packet.SerializePacket(writer, packetData);
                    MessagingManager.SendUnnamedMessageToAll(writer);
                    Debug.Log("sent");

                }
                else
                {
                    
                }

            }   

        }
    }
}