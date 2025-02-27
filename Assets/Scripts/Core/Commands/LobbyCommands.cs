﻿using System.Collections.Generic;
using System.Threading.Tasks;
using QFSW.QC;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;

namespace Core.Commands
{
    public static class LobbyCommands
    {
        private static readonly BetterLogger _logger = new(typeof(LobbyCommands));

        [Command]
        public static async Task CreateLobby(string name, int size = 1)
        {
            await LobbyManager.CreateLobby(name, size);
        }

        [Command]
        public static async Task QueryAllLobbies()
        {
            var lobbies = await LobbyManager.QueryLobbies();
            foreach (var lobby in lobbies.Results)
                _logger.LogInfo($"Lobby {lobby.Name}, id: {lobby.Id}, size: {lobby.MaxPlayers}");
        }

        [Command]
        public static async Task JoinLobby(string id)
        {
            await LobbyManager.JoinLobbyById(id);
        }

        [Command]
        public static async Task JoinLobbyFromName(string name)
        {
            var lobbies = await LobbyManager.QueryLobbies(new QueryLobbiesOptions
                                                          {
                                                              Count = 1,
                                                              Filters = new List<QueryFilter>
                                                                        {
                                                                            new(QueryFilter.FieldOptions.Name, name,
                                                                                QueryFilter.OpOptions.EQ)
                                                                        }
                                                          });
            await LobbyManager.JoinLobbyById(lobbies.Results[0]?.Id);
        }

        [Command]
        public static async Task SetUsername(string name)
        {
            await AuthenticationService.Instance.UpdatePlayerNameAsync(name);
        }
    }
}