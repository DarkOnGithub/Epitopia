using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Network.Lobby.Authentification;
using Network.Messages;
using Network.Messages;
using Network.Messages.Packets.Network;
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
        private readonly BetterLogger _logger = new(typeof(NetworkHandler));
        private NetworkManager _networkManager;

        private async void Awake()
        {
            _networkManager = GetComponent<NetworkManager>();

            await UnityServices.InitializeAsync();
            await Authentification.TrySignIn();
            PacketRegistry.RegisterPackets();

            _instance = this;
            _logger.LogInfo($"Connected as {AuthenticationService.Instance.PlayerId}");
            PlayerId = AuthenticationService.Instance.PlayerId;
            StartCoroutine(LobbyManager.HeartbeatLobby());
        }

      
    }
}