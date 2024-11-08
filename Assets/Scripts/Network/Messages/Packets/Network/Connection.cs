using Players;

namespace Network.Messages.Packets.Network
{
    public enum ConnectionState
    {
        Connecting = 0,
        Disconnecting = 1
    }

    public struct ConnectionMessage : IMessageData
    {
        public ConnectionState State;
        public string PlayerName;
        public string PlayerId;
        public ulong ClientId;
    }

    public class Connection : NetworkPacket<ConnectionMessage>
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