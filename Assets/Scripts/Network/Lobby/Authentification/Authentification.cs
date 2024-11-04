using System.Threading.Tasks;
using Core;
using Unity.Services.Authentication;

namespace Network.Lobby.Authentification
{
    public static class Authentification
    {
        private static BetterLogger _logger = new(typeof(Authentification));
        private static int _attempts = 10;

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
                _logger.LogError("Failed to sign in");
        }
    }
}