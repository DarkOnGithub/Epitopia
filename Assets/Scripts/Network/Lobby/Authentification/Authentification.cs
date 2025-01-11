using System.Threading.Tasks;
using Core;
using Unity.Services.Authentication;
using AuthenticationException = System.Security.Authentication.AuthenticationException;

namespace Network.Lobby.Authentification
{
    public static class Authentification
    {
        public static string LocalPlayerId;
        private static readonly BetterLogger _logger = new(typeof(Authentification));
        private static readonly int _attempts = 10;

        /// <summary>
        ///     Sign the local user to unity services
        /// </summary>
        /// <exception cref="AuthenticationException">Thrown if any error while trying to sign in</exception>
        public static async Task TrySignIn()
        {
            var attemps = 0;
            while (!AuthenticationService.Instance.IsSignedIn && attemps++ < _attempts)
            {
                _logger.LogInfo("Trying to sign in anonymously");
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }

            if (AuthenticationService.Instance.IsSignedIn)
                _logger.LogInfo("Signed in successfully");
            else
                throw new AuthenticationException("Failed to sign in");
            LocalPlayerId = AuthenticationService.Instance.PlayerId;
        }
    }
}