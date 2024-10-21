using System.Globalization;
using System.Threading.Tasks;
using Network.Lobby.Authentification;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class RelayManager : MonoBehaviour
{
    
    public async Task<string> StartHostWithRelay(int maxConnections=5)
    {
        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
            await Authentification.AuthentificateAnonymously();
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);
        
        var relayServerData = new RelayServerData(
            allocation.RelayServer.IpV4, 
            (ushort)allocation.RelayServer.Port, 
            allocation.AllocationIdBytes, 
            allocation.ConnectionData, 
            allocation.ConnectionData, 
            allocation.Key, 
            true);
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
        var joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
        return NetworkManager.Singleton.StartHost() ? joinCode : null;
    }

    async void Start()
    {
        var key = await StartHostWithRelay();
        Debug.Log(key);
    }

    void Update()
    {
        
    }
}
