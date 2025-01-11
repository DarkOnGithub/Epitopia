using Core;
using Network.Lobby.Authentification;
using Network.Messages;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace Network
{
    public class NetworkHandler : MonoBehaviour
    {
        [SerializeField] public string serverCode;
        private readonly BetterLogger _logger = new(typeof(NetworkHandler));
        private NetworkManager _networkManager;
        public static string PlayerId { get; private set; }
        public static NetworkHandler Instance { get; private set; }

        private async void Awake()
        {
            _networkManager = GetComponent<NetworkManager>();

            await UnityServices.InitializeAsync();
            await Authentification.TrySignIn();
            PacketRegistry.RegisterPackets();

            Instance = this;
            _logger.LogInfo($"Connected as {AuthenticationService.Instance.PlayerId}");
            PlayerId = AuthenticationService.Instance.PlayerId;
            StartCoroutine(LobbyManager.HeartbeatLobby());
        }
    }
}