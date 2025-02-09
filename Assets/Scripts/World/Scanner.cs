﻿using System;
using System.Collections;
using System.Collections.Generic;
using Events.Events;
using Network.Messages;
using Network.Messages.Packets.World;
using Players;
using Unity.Netcode;
using UnityEngine;
using World.Chunks;

namespace World
{
    public class Scanner : NetworkBehaviour
    {
        private const int SCAN_PADDING = 2;
        private const float SCAN_RATE = 20f;
        private const float SCAN_INTERVAL = 1f / SCAN_RATE;

        public static Scanner Instance;
        private static Coroutine _scannerCoroutine;

        private static readonly HashSet<Vector2Int> RequestedChunks = new();
        private static readonly HashSet<Chunk> LoadedChunks = new();
        private Vector2Int[] _scanPositionsBuffer;

        private int _scanRangeHorizontal;
        private int _scanRangeVertical;
        private WaitForSeconds _scanWaiter;

        private void Awake()
        {
            Instance = this;
        }

        public void InitializeScanner(Camera camera)
        {
            Debug.Log("Init scanner");
            InitializeScanRanges(camera);
            _scanWaiter = new WaitForSeconds(SCAN_INTERVAL);
            StartScheduler();
        }

        private void InitializeScanRanges(Camera camera)
        {

            var aspect = camera.aspect;
            var orthographicSize = camera.orthographicSize;

            Vector2 screenDimensions = new(
                orthographicSize * 2 * aspect,
                orthographicSize * 2
            );

            _scanRangeHorizontal = Mathf.CeilToInt(screenDimensions.x / Chunk.ChunkSize) / 2 + SCAN_PADDING;
            _scanRangeVertical = Mathf.CeilToInt(screenDimensions.y / Chunk.ChunkSize) / 2 + SCAN_PADDING;
            var maxPositions = _scanRangeHorizontal * 2 * _scanRangeVertical * 2;
            _scanPositionsBuffer = new Vector2Int[maxPositions];
        }


        public static void StartScheduler()
        {
            StopScheduler();
            _scannerCoroutine = Instance.StartCoroutine(Instance.ScanScheduler());
        }

        public static void StopScheduler()
        {
            if (_scannerCoroutine != null)
            {
                Instance.StopCoroutine(_scannerCoroutine);
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
            var chunksToRemove = new HashSet<Chunk>(LoadedChunks);
            var requestCount = 0;

            var basePosition = localPlayer.Position;

            for (var x = -_scanRangeHorizontal; x < _scanRangeHorizontal; x++)
            for (var y = -_scanRangeVertical; y < _scanRangeVertical; y++)
            {
                var chunkPosition = WorldUtils.FindNearestChunkPosition(
                    basePosition + new Vector2Int(x, y) * Chunk.ChunkSize
                );

                if (localPlayer.World.Query.TryGetChunk(chunkPosition, out var chunk))
                {
                    if (chunk != null)
                    {
                        chunk.Render();
                        LoadedChunks.Add(chunk);
                        chunksToRemove.Remove(chunk);
                        RequestedChunks.Remove(chunkPosition);
                    }
                }
                else if (!RequestedChunks.Contains(chunkPosition))
                {
                    _scanPositionsBuffer[requestCount++] = chunkPosition;
                    RequestedChunks.Add(chunkPosition);
                }
            }

            if (requestCount > 0) RequestChunks(localPlayer.World.Identifier, _scanPositionsBuffer, requestCount);

            foreach (var chunk in chunksToRemove)
            {
                LoadedChunks.Remove(chunk);
                chunk.Dispose();
            }
        }
    }
}