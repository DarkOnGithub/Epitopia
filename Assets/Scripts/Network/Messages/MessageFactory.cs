using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Core;
using JetBrains.Annotations;
using MessagePack;
using Network.Messages.Packets.Network;
using Network.Messages.Packets.Test;
using Network.Messages.Packets;
using Unity.Collections;
using Unity.Netcode;
using Unity.Services.Relay.Models;
using Utils;
using Debug = UnityEngine.Debug;
using Type = System.Type;

namespace Network.Messages
{
    public class MessageFactory : NetworkBehaviour
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


        public override void OnNetworkSpawn()
        {
            MessagingManager = NetworkManager.Singleton.CustomMessagingManager;
            MessagingManager.OnUnnamedMessage += OnUnnamedMessageReceived;

        }

        public override void OnNetworkDespawn()
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
            reader.ReadValueSafe(out int headerSize);
            if(!reader.TryBeginRead(headerSize))
                return;
            var headerBytes = new byte[headerSize];
            reader.ReadBytes(ref headerBytes, headerSize);
            var header = MessagePackSerializer.Deserialize<NetworkUtils.Header>(headerBytes);
            switch (header.SendingMode)
            {
                case (byte)SendingMode.ClientToClient:
                    if (NetworkManager.Singleton.IsHost)
                    {
                        var newHeader = NetworkUtils.GenerateHeader(SendingMode.ServerToClient, header.PacketId,
                                                                    header.Author, header.TargetIds);
                        reader.ReadValueSafe(out int bodyLength);
                        var body = new byte[bodyLength];
                        reader.ReadBytesSafe(ref body, bodyLength);
                        var buffer = new FastBufferWriter(sizeof(int) + newHeader.Length + sizeof(int) + bodyLength,
                                                          Allocator.Temp);
                        if (!buffer.TryBeginWrite(sizeof(int) + newHeader.Length + sizeof(int) + bodyLength))
                            return;
                        NetworkUtils.WriteBytesToWriter(ref buffer, newHeader);
                        NetworkUtils.WriteBytesToWriter(ref buffer, body);
                        SendBufferTo(buffer, SendingMode.ServerToClient, header.TargetIds);
                    }
                    return;
                case (byte)SendingMode.ClientToServer:
                    if(!NetworkManager.Singleton.IsHost)
                        return;
                    break;
            }
            if (!NetworkMessageIds.TryGetValue(header.PacketId, out var networkMessage))
            {
                Logger.LogWarning($"Received unknown packet with id {header.PacketId.ToHex()}");
                return;
            }

            reader.ReadValueSafe(out int bufferSize);
            if(!reader.TryBeginRead(bufferSize))
                return;
            var buff = new byte[bufferSize];
            reader.ReadBytes(ref buff, bufferSize);
            networkMessage.OnPacketReceived((IMessageData)MessagePackSerializer.Deserialize(networkMessage.MessageType, buff));
        }

        public static void SendPacket<T>(SendingMode mode, T packetData, [CanBeNull] ulong[] clientIds = null, ulong? author = null) 
            where T : IMessageData
        {
            if (!NetworkMessageTypes.TryGetValue(packetData.GetType(), out var networkMessage))
            {
                Logger.LogWarning($"Failed to send packet {packetData.GetType().Name} as it is not registered");
                return;
            }

            networkMessage.SendMessageTo(packetData, mode, author.GetValueOrDefault(NetworkManager.Singleton.LocalClientId), clientIds);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="clientIds">ClientIds = null means To All</param>
        public static void SendBufferTo(FastBufferWriter buffer, SendingMode mode, [CanBeNull] ulong[] clientIds = null)
        {
            switch (mode)
            {
                case SendingMode.ClientToClient:
                    MessagingManager.SendUnnamedMessage(NetworkManager.ServerClientId, buffer, NetworkDelivery.ReliableFragmentedSequenced);
                    break;
                case SendingMode.ClientToServer:
                    MessagingManager.SendUnnamedMessage(NetworkManager.ServerClientId, buffer, NetworkDelivery.ReliableFragmentedSequenced);
                    break;
                case SendingMode.ServerToClient:
                    if (clientIds == null)
                        MessagingManager.SendUnnamedMessageToAll(buffer, NetworkDelivery.ReliableFragmentedSequenced);
                    else
                        MessagingManager.SendUnnamedMessage(clientIds, buffer, NetworkDelivery.ReliableFragmentedSequenced);
                    break;
            }

            buffer.Dispose();
        }
        public static short GeneratePacketId(NetworkMessageIdenfitier idenfitier) 
        {
            if (!_packetCount.ContainsKey(idenfitier.GetType()))
                _packetCount[idenfitier.GetType()] = 0;
            return (short)((byte)idenfitier << 8 | _packetCount[idenfitier.GetType()]++);
        }
        
    }
}