using System;
using Unity.Netcode;
using UnityEngine;

namespace Network
{
    public class NetworkHandler : MonoBehaviour
    {
        private NetworkManager _networkManager;

        private void Awake()
        {
            _networkManager = GetComponent<NetworkManager>();
            _networkManager.OnClientConnectedCallback += id => Debug.Log($"Client connected: {id}");
        }
    }
}