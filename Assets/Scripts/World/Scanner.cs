// using System;
// using System.Collections;
// using System.Collections.Generic;
// using Events.Events;
// using Network.Messages;
// using Network.Messages.Packets.World;
// using Players;
// using Unity.Netcode;
// using UnityEngine;
// using World.Chunks;
//
// namespace World
// {
//     public class Scanner : NetworkBehaviour
//     {
//         private const int SCAN_PADDING = 2;
//         private const float SCAN_RATE = 20f;
//         private const float SCAN_INTERVAL = 1f / SCAN_RATE;
//
//         public static Scanner Instance;
//         private static Coroutine _scannerCoroutine;
//
//         private static readonly HashSet<Vector2Int> RequestedChunks = new();
//         private static readonly HashSet<Chunk> LoadedChunks = new();
//         private Vector2Int[] _scanPositionsBuffer;
//
//         private int _scanRangeHorizontal;
//         private int _scanRangeVertical;
//         private WaitForSeconds _scanWaiter;
//
//         private void Awake()
//         {
//             Instance = this;
//         }
//
//         public void InitializeScanner(Camera camera)
//         {
//             Debug.Log("Init scanner");
//             Debug.Log(camera);
//             InitializeScanRanges(camera);
//             _scanWaiter = new WaitForSeconds(SCAN_INTERVAL);
//             StartScheduler();
//         }
//
//         private void InitializeScanRanges(Camera camera)
//         {
//             var aspect = camera.aspect;
//             var orthographicSize = camera.orthographicSize;
//
//             Vector2 screenDimensions = new(
//                 orthographicSize * 2 * aspect,
//                 orthographicSize * 2
//             );
//
//             _scanRangeHorizontal = Mathf.CeilToInt(screenDimensions.x / Chunk.ChunkSize) / 2 + SCAN_PADDING;
//             _scanRangeVertical = Mathf.CeilToInt(screenDimensions.y / Chunk.ChunkSize) / 2 + SCAN_PADDING;
//             var maxPositions = _scanRangeHorizontal * 2 * _scanRangeVertical * 2;
//             _scanPositionsBuffer = new Vector2Int[maxPositions];
//         }
//
//
//         public static void StartScheduler()
//         {
//             StopScheduler();
//             _scannerCoroutine = Instance.StartCoroutine(Instance.ScanScheduler());
//         }
//
//         public static void StopScheduler()
//         {
//             if (_scannerCoroutine != null)
//             {
//                 Instance.StopCoroutine(_scannerCoroutine);
//                 _scannerCoroutine = null;
//             }
//         }
//
//         private static void RequestChunks(WorldIdentifier worldId, Vector2Int[] positions, int count)
//         {
//             if (count == 0) return;
//             var packet = new ChunkRequestMessage
//                          {
//                              Positions = new Vector2Int[count],
//                              Type = ChunkRequestType.Request,
//                              World = worldId
//                          };
//
//             Array.Copy(positions, packet.Positions, count);
//
//             MessageFactory.SendPacket(
//                 SendingMode.ClientToServer,
//                 packet,
//                 null,
//                 null,
//                 NetworkDelivery.ReliableFragmentedSequenced
//             );
//         }
//
//         private IEnumerator ScanScheduler()
//         {
//             while (true)
//             {
//                 Scan();
//                 yield return _scanWaiter;
//             }
//         }
//
//         private void Scan()
//         {
//             var localPlayer = PlayerManager.LocalPlayer;
//             if (localPlayer?.World == null) return;
//             var chunksToRemove = new HashSet<Chunk>(LoadedChunks);
//             var requestCount = 0;
//
//             var basePosition = localPlayer.Position;
//
//             for (var x = -_scanRangeHorizontal; x < _scanRangeHorizontal; x++)
//             for (var y = -_scanRangeVertical; y < _scanRangeVertical; y++)
//             {
//                 var chunkPosition = WorldUtils.FindNearestChunkPosition(
//                     basePosition + new Vector2Int(x, y) * Chunk.ChunkSize
//                 );
//
//                 if (localPlayer.World.Query.TryGetChunk(chunkPosition, out var chunk))
//                 {
//                     if (chunk != null)
//                     {
//                         chunk.Render();
//                         LoadedChunks.Add(chunk);
//                         chunksToRemove.Remove(chunk);
//                         RequestedChunks.Remove(chunkPosition);
//                     }
//                 }
//                 else if (!RequestedChunks.Contains(chunkPosition))
//                 {
//                     _scanPositionsBuffer[requestCount++] = chunkPosition;
//                     RequestedChunks.Add(chunkPosition);
//                 }
//             }
//
//             if (requestCount > 0) RequestChunks(localPlayer.World.Identifier, _scanPositionsBuffer, requestCount);
//
//             foreach (var chunk in chunksToRemove)
//             {
//                 LoadedChunks.Remove(chunk);
//                 chunk.Dispose();
//             }
//         }
//     }
// }

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Network.Messages.Packets.World;
using Players;
using UnityEngine;
using Utils;
using World.Chunks;

namespace World
{
    public class Scanner : MonoBehaviour
    {
        public static Scanner Instance;
        private Camera _camera;
        private const int ScanPadding = 2;
        private Vector2Int _scanRange = new();
        private float _orthographicSize = 0f;
        private readonly HashSet<Vector2Int> _scannedPositions = new();

        public Scanner()
        {
            Instance = this;
        }

        
        private void CalculateScanDimension()
        {
            var aspect = _camera.aspect;
            var orthographicSize = _camera.orthographicSize;
            if (Mathf.Abs(orthographicSize - _orthographicSize) < 0.01f)
                return;
            _orthographicSize = orthographicSize;
            Vector2 screenDimensions = new(
                orthographicSize * 2 * aspect,
                orthographicSize * 2
            );

            var horizontal = Mathf.CeilToInt(screenDimensions.x / Chunk.ChunkSize) / 2 + ScanPadding;
            var vertical = Mathf.CeilToInt(screenDimensions.y / Chunk.ChunkSize) / 2 + ScanPadding;
            _scanRange = new(horizontal, vertical);
        }
        
        
        public void SetCamera(Camera camera)
        {
            _camera = camera;
            CalculateScanDimension();
        }
        
        public void FixedUpdate()
        {
            if (_camera is null) return;
            CalculateScanDimension();
            var localPlayer = PlayerManager.LocalPlayer;
            var world = localPlayer.World;
            if (localPlayer == null || world == null) return;
            
            var clientHandler = world.ClientHandler;
            var query = clientHandler.Query;
            
            var playerPosition = localPlayer.Position;
            var scannedPositionsCpy = new HashSet<Vector2Int>(_scannedPositions);
            
            _scannedPositions.Clear();
            for(int x = -_scanRange.x; x <= _scanRange.x; x++)
            for (int y = -_scanRange.y; y <= _scanRange.y; y++)
            {
                var chunkPosition = VectorUtils.GetNearestChunkPosition(
                     playerPosition + new Vector2Int(x, y) * Chunk.ChunkSize
                );
                if (query.TryGetChunk(chunkPosition, out var chunk))
                    chunk.Render();
                else
                    WorldsManager.Instance.EnqueueChunkRequest(chunkPosition, localPlayer.World.Identifier);
                
                _scannedPositions.Add(chunkPosition);
                scannedPositionsCpy.Remove(chunkPosition);
            }

            foreach (var position in scannedPositionsCpy)
                WorldsManager.Instance.EnqueueChunkRequest(position, world.Identifier, ChunkRequestType.Drop);
            
        }
    }
}