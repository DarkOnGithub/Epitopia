using MessagePack;
using Players;
using UnityEngine;

namespace Network.Messages.Packets.Network
{
    public enum ConnectionState
    {
        Connecting = 0,
        Disconnecting = 1
    }
    [MessagePackObject]
    public struct ConnectionMessage : IMessageData
    {
        [Key(0)]
        public ConnectionState State;
        [Key(1)]
        public string PlayerName;
        [Key(2)]
        public string PlayerId;
        [Key(3)]
        public ulong ClientId;
    }

    public class ConnectionPacket : NetworkPacket<ConnectionMessage>
    {
        public override NetworkMessageIdenfitier Identifier { get; } = NetworkMessageIdenfitier.Network;
        protected override void OnPacketReceived(NetworkUtils.Header header, ConnectionMessage body)
        {
            switch (body.State)
            {
                case ConnectionState.Connecting:
                    PlayerManager.OnPlayerConnected(body);
                    break;
                case ConnectionState.Disconnecting:
                    PlayerManager.OnPlayerDisconnected(body);
                    break;
            }
            Logger.LogInfo($"Received connection information from {body.PlayerName} ({body.ClientId}); state ({body.State})");
            
        }
    }
}