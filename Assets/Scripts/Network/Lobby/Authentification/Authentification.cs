using System.Threading.Tasks;
using Unity.Services.Authentication;

namespace Network.Lobby.Authentification
{
    public static class Authentification
    {
        public static async Task AuthentificateAnonymously()
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }
}