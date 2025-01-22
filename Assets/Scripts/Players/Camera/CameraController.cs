using System;
using Unity.Netcode;
using UnityEngine;

namespace Players.Camera
{
    public class CameraController : NetworkBehaviour
    {
        private UnityEngine.Camera _camera;
        private float _yOffset = 5f;
        private Player _localPlayer;
        private Vector2 _lastPosition = new();

        public override void OnNetworkSpawn()
        {
            _camera = GetComponent<UnityEngine.Camera>();
            _localPlayer = PlayerManager.LocalPlayer;
        }

        // private void Update()
        // {
        //     if(_lastPosition == _localPlayer.Position)
        //         return;
        //     _lastPosition = _localPlayer.Position;
        //     
        //     _camera.transform.position = new Vector3(_localPlayer.Position.x, _localPlayer.Position.y + _yOffset, -10);
        // }
    }
}