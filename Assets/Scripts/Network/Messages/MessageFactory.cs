using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using JetBrains.Annotations;
using MessagePack;
using Network.Messages.Packets;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using Utils;
using Type = System.Type;

namespace Network.Messages
{
    public class MessageFactory : NetworkBehaviour
    {
        private static readonly Dictionary<Type, byte> PacketsCount = new();
        private static readonly Dictionary<int, INetworkMessage> NetworkMessageIds = new();
        private static readonly Dictionary<Type, INetworkMessage> NetworkMessageTypes = new();
        public static CustomMessagingManager MessagingManager;
        private static readonly BetterLogger Logger = new(typeof(MessageFactory));

        public static bool IsInitialized => MessagingManager != null;


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
            if (!reader.TryBeginRead(headerSize))
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
                        SendBufferTo(buffer, SendingMode.ServerToClient, NetworkDelivery.ReliableFragmentedSequenced,
                                     header.TargetIds);
                    }

                    return;
            }

            if (!NetworkMessageIds.TryGetValue(header.PacketId, out var networkMessage))
            {
                Logger.LogWarning($"Received unknown packet with id {header.PacketId.ToHex()}");
                return;
            }

            reader.ReadValueSafe(out int bufferSize);
            if (!reader.TryBeginRead(bufferSize))
                return;
            var buff = new byte[bufferSize];
            reader.ReadBytes(ref buff, bufferSize);
            networkMessage.OnPacketReceived(
                header, (IMessageData)MessagePackSerializer.Deserialize(networkMessage.MessageType, buff));
        }

        public static void SendPacket<T>(SendingMode mode, T packetData, [CanBeNull] ulong[] clientIds = null,
            ulong? author = null, NetworkDelivery delivery = NetworkDelivery.Reliable)
            where T : IMessageData
        {
            if (!NetworkMessageTypes.TryGetValue(packetData.GetType(), out var networkMessage))
            {
                Logger.LogWarning($"Failed to send packet {packetData.GetType().Name} as it is not registered");
                return;
            }

            networkMessage.SendMessageTo(packetData, mode,
                                         author.GetValueOrDefault(NetworkManager.Singleton.LocalClientId), clientIds,
                                         delivery);
        }


        public static void SendBufferTo(FastBufferWriter buffer, SendingMode mode, NetworkDelivery delivery,
            [CanBeNull] ulong[] clientIds = null)
        {
            switch (mode)
            {
                case SendingMode.ClientToClient:
                    MessagingManager.SendUnnamedMessage(NetworkManager.ServerClientId, buffer,
                                                        delivery);
                    break;
                case SendingMode.ClientToServer:
                    MessagingManager.SendUnnamedMessage(NetworkManager.ServerClientId, buffer,
                                                        delivery);
                    break;
                case SendingMode.ServerToClient:
                    if (clientIds == null)
                        MessagingManager.SendUnnamedMessageToAll(buffer, delivery);
                    else
                        MessagingManager.SendUnnamedMessage(clientIds, buffer,
                                                            delivery);
                    break;
            }

            buffer.Dispose();
        }

        public static int GeneratePacketId(NetworkMessageIdenfitier identifier)
        {
            var identifierType = identifier.GetType();
            if (!PacketsCount.ContainsKey(identifierType))
                PacketsCount[identifierType] = 0;
            return ((byte)identifier << 24) | (PacketsCount[identifierType]++ & 0xFFFFFF);
        }
    }
}