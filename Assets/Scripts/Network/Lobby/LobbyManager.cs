using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using Core;
using JetBrains.Annotations;
using Network;
using Network.Lobby;
using Network.Lobby.Authentification;
using Network.Messages;
using Network.Messages.Packets.Network;
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

    public static async Task CreateLobby(string name, int maxPlayers = 1)
    {
        try
        {
            var code = await RelayManager.StartHost(maxPlayers);
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
            await RelayManager.StartClient(lobby.Data[JOIN_CODE].Value);
            _logger.LogWarning($"Joined lobby {CurrentLobby.Name}");
        }
        catch (LobbyServiceException e)
        {
            _logger.LogWarning(e);
        }
    }
}