using System;
using Core;
using Network.Lobby.Authentification;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace Network
{
    public class NetworkHandler : MonoBehaviour
    {
        public static string PlayerId { get; private set; }

        private static NetworkHandler _instance;
        public static NetworkHandler Instance => _instance;
        [SerializeField] public string serverCode;
        private BetterLogger _logger = new(typeof(NetworkHandler));
        private NetworkManager _networkManager;

        private async void Awake()
        {
            if (_instance != null && _instance != this)
                Destroy(gameObject);
            else
                _instance = this;

            _networkManager = GetComponent<NetworkManager>();
            await UnityServices.InitializeAsync();
            await Authentification.TrySignIn();
            _logger.LogInfo($"Connected as {AuthenticationService.Instance.PlayerId}");
            PlayerId = AuthenticationService.Instance.PlayerId;
            StartCoroutine(LobbyManager.HeartbeatLobby());
        }
    }
}