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
        public const int ScanPadding = 2;
        public static int ScanRangeHorizontal;
        public static int ScanRangeVertical;
        public const float ScanRate = 20f;
        private static Coroutine _scheduler;
        public static Scanner Instance;
        public static HashSet<Vector2Int> RequestedChunks = new HashSet<Vector2Int>();
        public static HashSet<Chunk> LoadedChunks = new();
        public void Start()
        {
            Instance = this;
            Vector2 screenDimensions = new Vector2(
                Camera.main.orthographicSize * 2 * Camera.main.aspect,
                Camera.main.orthographicSize * 2
            );

            ScanRangeHorizontal = Mathf.CeilToInt(screenDimensions.x / Chunk.ChunkSize)  / 2 + ScanPadding;
            ScanRangeVertical = Mathf.CeilToInt(screenDimensions.y / Chunk.ChunkSize) / 2 + ScanPadding ;
        }
        [SubscribeEvent]
        public static void OnPlayerAdded(PlayerAddedEvent e)
        {
            if(e.Player.ClientId == NetworkManager.Singleton.LocalClientId)
                StartScheduler();
        }
        public static void StartScheduler()
        {
            _scheduler = Instance.StartCoroutine(Instance.Scheduler());
        }
        
        public static void StopScheduler()
        {
            Instance.StopCoroutine(_scheduler);
            _scheduler = null;
        }
        
        
        private static void RequestChunks(WorldIdentifier worldId, Vector2Int[] positions)
        {
            var packet = new ChunkRequestMessage
            {
                Positions = positions,
                Type = ChunkRequestType.Request,
                World = worldId
            };
            MessageFactory.SendPacket(SendingMode.ClientToServer, packet, null, null, NetworkDelivery.ReliableFragmentedSequenced);
        }
        IEnumerator Scheduler()
        {
            var waiter = new WaitForSeconds( 1 / ScanRate);
            while (true)
            {
                yield return waiter;
                Scan();
            }
        }
        
        private void Scan()
        {
            var localPlayer = PlayerManager.LocalPlayer;
            var loadedChunks = new HashSet<Chunk>(LoadedChunks);
            List<Vector2Int> chunksToRequest = new List<Vector2Int>(); 

            for(int x = -ScanRangeHorizontal; x < ScanRangeHorizontal; x++)
            {
                for(int y = -ScanRangeVertical; y < ScanRangeVertical; y++)
                {
                    Vector2Int chunkPosition = WorldQuery.FindNearestChunkPosition(localPlayer.Position + new Vector2Int(x, y) * Chunk.ChunkSize);
                    if(localPlayer.World.TryGetChunk(chunkPosition, out var chunk))
                    {
                        if (chunk == null)
                            continue;
                        chunk.TryDraw();
                        LoadedChunks.Add(chunk);
                        loadedChunks.Remove(chunk);
                    }
                    else if(!RequestedChunks.Contains(chunkPosition) )
                    {
                        chunksToRequest.Add(chunkPosition);
                        RequestedChunks.Add(chunkPosition);
                    }
                }   
            }
            RequestChunks(localPlayer.World.Identifier, chunksToRequest.ToArray());
            foreach (var chunk in loadedChunks)
            {
                //chunk.TryUnload();
                LoadedChunks.Remove(chunk);
            }
        }
    }
}