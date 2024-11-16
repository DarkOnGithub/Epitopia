using System;
using System.Collections;
using System.Collections.Generic;
using Events.EventHandler;
using Events.Events;
using Network.Messages;
using Network.Messages.Packets.World;
using Players;
using Unity.Netcode;
using UnityEngine;
using World.Chunks;

namespace World
{
    public class Scanner : MonoBehaviour
    {
        private const int SCAN_PADDING = 2;
        private const float SCAN_RATE = 20f;
        private const float SCAN_INTERVAL = 1f / SCAN_RATE;

        private static Scanner _instance;
        private static Coroutine _scannerCoroutine;
        
        private static readonly HashSet<Vector2Int> RequestedChunks = new();
        private static readonly HashSet<Chunk> LoadedChunks = new();
        
        private int _scanRangeHorizontal;
        private int _scanRangeVertical;
        private WaitForSeconds _scanWaiter;
        private Vector2Int[] _scanPositionsBuffer;

        public void Start()
        {
            if (_instance != null)
            {
                Debug.LogError("Multiple Scanner instances detected!");
                Destroy(this);
                return;
            }
            
            _instance = this;
            InitializeScanRanges();
            _scanWaiter = new WaitForSeconds(SCAN_INTERVAL);
        }

        private void InitializeScanRanges()
        {
            if (Camera.main == null) return;
            
            float aspect = Camera.main.aspect;
            float orthographicSize = Camera.main.orthographicSize;
            
            Vector2 screenDimensions = new(
                orthographicSize * 2 * aspect,
                orthographicSize * 2
            );

            _scanRangeHorizontal = Mathf.CeilToInt(screenDimensions.x / Chunk.ChunkSize) / 2 + SCAN_PADDING;
            _scanRangeVertical = Mathf.CeilToInt(screenDimensions.y / Chunk.ChunkSize) / 2 + SCAN_PADDING;
            
            int maxPositions = (_scanRangeHorizontal * 2) * (_scanRangeVertical * 2);
            _scanPositionsBuffer = new Vector2Int[maxPositions];
        }

        [SubscribeEvent]
        public static void OnPlayerAdded(PlayerAddedEvent e)
        {
            if (e.Player.ClientId == NetworkManager.Singleton.LocalClientId)
            {
                StartScheduler();
            }
        }

        public static void StartScheduler()
        {
            StopScheduler();
            _scannerCoroutine = _instance.StartCoroutine(_instance.ScanScheduler());
        }

        public static void StopScheduler()
        {
            if (_scannerCoroutine != null)
            {
                _instance.StopCoroutine(_scannerCoroutine);
                _scannerCoroutine = null;
            }
        }
        
        private static void RequestChunks(WorldIdentifier worldId, Vector2Int[] positions, int count)
        {
            if (count == 0) return;

            var packet = new ChunkRequestMessage
            {
                Positions = new Vector2Int[count],
                Type = ChunkRequestType.Request,
                World = worldId
            };
            
            Array.Copy(positions, packet.Positions, count);
            
            MessageFactory.SendPacket(
                SendingMode.ClientToServer, 
                packet, 
                null, 
                null, 
                NetworkDelivery.ReliableFragmentedSequenced
            );
        }

        private IEnumerator ScanScheduler()
        {
            while (true)
            {
                Scan();
                yield return _scanWaiter;
            }
        }

        private void Scan()
        {
            var localPlayer = PlayerManager.LocalPlayer;
            if (localPlayer?.World == null) return;

            var loadedChunksCopy = new HashSet<Chunk>(LoadedChunks);
            int requestCount = 0;

            Vector2 basePosition = localPlayer.Position;
            
            // Use pre-allocated buffer instead of creating new list
            for (int x = -_scanRangeHorizontal; x < _scanRangeHorizontal; x++)
            {
                for (int y = -_scanRangeVertical; y < _scanRangeVertical; y++)
                {
                    Vector2Int chunkPosition = WorldUtils.FindNearestChunkPosition(
                        basePosition + new Vector2Int(x, y) * Chunk.ChunkSize
                    );

                    if (localPlayer.World.Query.TryGetChunk(chunkPosition, out var chunk))
                    {
                        if (chunk != null)
                        {
                            //    chunk.TryDraw();
                            LoadedChunks.Add(chunk);
                            loadedChunksCopy.Remove(chunk);
                        }
                    }
                    else if (!RequestedChunks.Contains(chunkPosition))
                    {
                        _scanPositionsBuffer[requestCount++] = chunkPosition;
                        RequestedChunks.Add(chunkPosition);
                    }
                }
            }

            // Only send request if we have chunks to request
            if (requestCount > 0)
            {
                RequestChunks(localPlayer.World.Identifier, _scanPositionsBuffer, requestCount);
            }

            // Cleanup unneeded chunks
            foreach (var chunk in loadedChunksCopy)
            {
                chunk.Destroy();
                LoadedChunks.Remove(chunk);
            }
        }
    }
}