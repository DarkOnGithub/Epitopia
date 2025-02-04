using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;
using World;
using Random = System.Random;

namespace Players.Camera
{
    public class CameraController : NetworkBehaviour
    {
        private UnityEngine.Camera _camera;
        private GameObject _lightMap;
        private Tilemap _mainTilemap;
        public override void OnNetworkSpawn()
        {
            _camera = GetComponent<UnityEngine.Camera>();
            var lightMapSource = Resources.Load<GameObject>("Prefabs/Lightmap");
            var lightMap = Instantiate(lightMapSource);
            _lightMap = lightMap;
            _mainTilemap = WorldManager.Instance.worldTilemap;
            var texture = new Texture2D(256, 256);
            lightMap.GetComponent<SpriteRenderer>().material.SetTexture("_Lightmap", texture);
            texture.filterMode = FilterMode.Point;
            GenerateRandomTexture(texture, 256, 256);
        }

        private void GenerateRandomTexture(Texture2D texture, int width, int height)
        {
                        
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var color = new Color(0, 0, 0, 1 - new Random().Next(0, 15) / 15f);
                    texture.SetPixel(x, y, color);
                }
            }
            texture.Apply();
        }

        private void FixedUpdate()
        {
            if (_lightMap)
            {
                Vector2 targetPosition = (Vector2)transform.position;
                // Snap to the closest tile
                Vector3Int cellPosition = _mainTilemap.WorldToCell(targetPosition);
                var position = _mainTilemap.GetCellCenterWorld(cellPosition);
                _lightMap.transform.position = new Vector3(position.x, position.y, -10);   
            }
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