using System;
using Core;
using Unity.Netcode;
using UnityEngine;

namespace Network
{
    public class NetworkHandler : MonoBehaviour
    {
        private BetterLogger _logger = new(typeof(NetworkHandler));
        private NetworkManager _networkManager;

        private void Awake()
        {
            _networkManager = GetComponent<NetworkManager>();
            _networkManager.OnClientConnectedCallback += OnPlayerJoin;
        }

        private void OnPlayerJoin(ulong id)
        {
            _logger.LogInfo($"Player {id} joined the game");
            _logger.LogInfo(_networkManager.ConnectedClients.Count);
        }
    }
}