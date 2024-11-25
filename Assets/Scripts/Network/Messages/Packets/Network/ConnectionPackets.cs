using System.Threading.Tasks;
using MessagePack;
using Players;
using Unity.Netcode;
using Unity.Services.Authentication;
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
        public delegate void OnPlayerAdded(ConnectionMessage message);
        public static event OnPlayerAdded OnPlayerAddedCallback;
        public delegate void OnPlayerRemoved(ConnectionMessage message);
        public static event OnPlayerRemoved OnPlayerRemovedCallback;
        public override NetworkMessageIdenfitier Identifier { get; } = NetworkMessageIdenfitier.Network;
        protected override void OnPacketReceived(NetworkUtils.Header header, ConnectionMessage body)
        {
            switch (body.State)
            {
                case ConnectionState.Connecting:
                    OnPlayerAddedCallback?.Invoke(body);
                    break;
                case ConnectionState.Disconnecting:
                    OnPlayerRemovedCallback?.Invoke(body);
                    break;
            }

            Logger.LogInfo($"Received connection information from {body.PlayerName} ({body.ClientId}); state ({body.State})");
        }


        public static async Task TrySendPacket()
        {
            while (!MessageFactory.IsInitialized)
            {
                await Task.Delay(100);
            }
            MessageFactory.SendPacket(SendingMode.ClientToClient, new ConnectionMessage
                                                                  {
                                                                      State = ConnectionState.Connecting,
                                                                      PlayerName = await AuthenticationService.Instance.GetPlayerNameAsync(),
                                                                      PlayerId = AuthenticationService.Instance.PlayerId,
                                                                      ClientId = NetworkManager.Singleton.LocalClientId
                                                                  });
        }
    }
}