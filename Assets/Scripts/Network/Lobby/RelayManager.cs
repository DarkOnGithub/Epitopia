using System.Threading.Tasks;
using Core;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Relay;

namespace Network.Lobby
{
    public static class RelayManager
    {
        private static readonly BetterLogger _logger = new(typeof(RelayManager));


        public static async Task StartClient(string joinCode)
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
        ///     Create a new relay allocation then start the host
        /// </summary>
        /// <param name="maxPlayers">Max amount of player in the allocation</param>
        /// <returns>Return a join code</returns>
        public static async Task<string> StartHost(int maxPlayers)
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
    }
}