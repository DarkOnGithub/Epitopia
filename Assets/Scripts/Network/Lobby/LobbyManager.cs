using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using Core;
using JetBrains.Annotations;
using Network;
using Network.Lobby.Authentification;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public static class LobbyManager
{
    private const string JOIN_CODE = "JOIN_CODE";
    [CanBeNull] public static Lobby CurrentLobby { get; private set; }
    private static BetterLogger _logger = new(typeof(LobbyManager));
    /// <summary>
    /// Infinite loop that sends a heartbeat ping to the lobby every 15 seconds
    /// </summary>
    public static IEnumerator HeartbeatLobby()
    {
        var waiter = new WaitForSeconds(15);
        while (true)
        {
            yield return waiter;
            if (CurrentLobby == null || CurrentLobby.HostId != NetworkHandler.PlayerId)
                continue;
            var task = LobbyService.Instance.SendHeartbeatPingAsync(CurrentLobby.Id);
            yield return new WaitUntil(() => task.IsCompleted);
        }
    }
    /// <summary>
    /// Join the relay using a join code then start the client
    /// </summary>
    /// <param name="joinCode">The join code</param>
    private static async Task StartClient(string joinCode)
    {
        try
        {
            var allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(
                allocation.RelayServer.IpV4,
                (ushort)allocation.RelayServer.Port,
                allocation.AllocationIdBytes,
                allocation.Key,
                allocation.ConnectionData,
                allocation.HostConnectionData
            );
            NetworkManager.Singleton.StartClient();
        }
        catch (RelayServiceException e)
        {
            _logger.LogWarning(e);
        }
    }
    /// <summary>
    /// Create a new relay allocation then start the host
    /// </summary>
    /// <param name="maxPlayers">Max amount of player in the allocation</param>
    /// <returns>Return a join code</returns>
    private static async Task<string> StartHost(int maxPlayers)
    {
        try
        {
            var allocation = await RelayService.Instance.CreateAllocationAsync(maxPlayers);
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(
                allocation.RelayServer.IpV4,
                (ushort)allocation.RelayServer.Port,
                allocation.AllocationIdBytes,
                allocation.Key,
                allocation.ConnectionData
            );
            NetworkManager.Singleton.StartHost();
            return await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
        }
        catch (RelayServiceException e)
        {
            _logger.LogWarning(e);
            return null;
        }
    }
    /// <summary>
    /// Create a lobby with a name of size maxPlayers
    /// </summary>
    /// <param name="name">The server name</param>
    /// <param name="maxPlayers">Max size of the server</param>
    public static async Task CreateLobby(string name, int maxPlayers = 1)
    {
        try
        {
            var code = await StartHost(maxPlayers);
            if (code == null)
                return;
            var lobbyOptions = new CreateLobbyOptions
            {
                Data = new Dictionary<string, DataObject>
                {
                    { JOIN_CODE, new DataObject(DataObject.VisibilityOptions.Public, code) }
                }
            };
            var lobby = await LobbyService.Instance.CreateLobbyAsync(name, maxPlayers, lobbyOptions);
            CurrentLobby = lobby;
            Debug.Log($"Lobby <{lobby.Name}> created with id {lobby.Id}, join code {code}");
        }
        catch (RelayServiceException e)
        {
            _logger.LogWarning(e);
        }
        catch (LobbyServiceException e)
        {
            _logger.LogWarning(e);
        }
    }
    /// <summary>
    /// Query lobbies following the optional parameters options
    /// No options will return all lobbies
    /// </summary>
    /// <param name="options">Optional parameter used to query lobbies</param>
    /// <returns>The result of the query</returns>
    public static async Task<QueryResponse> QueryLobbies(QueryLobbiesOptions options = null)
    {
        try
        {
            return await LobbyService.Instance.QueryLobbiesAsync(options);
        }
        catch (LobbyServiceException e)
        {
            _logger.LogWarning(e);
            return null;
        }
    }
    /// <summary>
    /// Join a lobby by its id
    /// </summary>
    /// <param name="lobbyId">The lobby id (not the relay id)</param>
    public static async Task JoinLobbyById(string lobbyId)
    {
        try
        {
            var lobby = await LobbyService.Instance.GetLobbyAsync(lobbyId);
            if (lobby.Players.Count >= lobby.MaxPlayers)
            {
                _logger.LogWarning($"Lobby {lobby.Name} is full");
                return;
            }

            CurrentLobby = lobby;
            await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId);
            await StartClient(lobby.Data[JOIN_CODE].Value);
            _logger.LogWarning($"Joined lobby {CurrentLobby.Name}");
        }
        catch (LobbyServiceException e)
        {
            _logger.LogWarning(e);
        }
    }
}