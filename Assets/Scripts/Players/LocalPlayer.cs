using Unity.Netcode;
using World;

namespace Players
{
    public class LocalPlayer : NetworkBehaviour
    {
        private NetworkObject _networkObject;
        private UnityEngine.Camera _camera;
        public override void OnNetworkSpawn()
        {
            if (!IsOwner)
            {
                GetComponent<PlayerController>().enabled = false;
                //GetComponent<Camera.CameraController>().enabled = false;
                GetComponent<UnityEngine.Camera>().enabled = false;
                return;
            }
            _camera = GetComponent<UnityEngine.Camera>();
            _networkObject = GetComponent<NetworkObject>();
            Scanner.Instance.InitializeScanner(_camera);
        }
    }
}