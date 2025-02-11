// using System;
// using System.Collections;
// using System.Collections.Concurrent;
// using System.Collections.Generic;
// using System.Threading;
// using System.Threading.Tasks;
// using UnityEngine;
// using UnityEngine.Tilemaps;
// using World.Chunks;
//
// namespace World
// {
//     public class WorldManager : MonoBehaviour
//     {
//         private const int MaxChunksPerTick = 20;
//         private const int MaxGenerationPerTick = 20;
//         private static readonly Dictionary<WorldIdentifier, Type> _worlds = new();
//         public static readonly ConcurrentQueue<Chunk> ChunkSenderQueue = new();
//         private static readonly Queue<Chunk> ToGenerateChunksQueue = new();
//
//         [SerializeField] public Tilemap worldTilemap;
//         [SerializeField] public Tilemap backgroundTilemap;
//         [SerializeField] public Tilemap vegetationTilemap;
//
//         [SerializeField] public Grid Grid;
//         public static WorldManager Instance { get; private set; }
//         public static ConcurrentDictionary<WorldIdentifier, AbstractWorld> Worlds { get; } = new();
//
//         private void Awake()
//         {
//             RegisterWorlds();
//             Instance = this;
//             StartCoroutine(ChunkGenerator());
//         }
//
//         private void RegisterWorlds()
//         {
//             RegisterWorld(typeof(Overworld), WorldIdentifier.Overworld);
//         }
//
//         public static void RegisterWorld(Type world, WorldIdentifier identifier)
//         {
//             _worlds[identifier] = world;
//         }
//
//         public static void LoadWorlds()
//         {
//             UnloadWorlds();
//             foreach (var world in _worlds)
//             {
//                 var instance = (AbstractWorld)Activator.CreateInstance(world.Value, world.Key);
//                 Worlds[world.Key] = instance;
//             }
//         }
//
//         public static void UnloadWorlds()
//         {
//             foreach (var world in Worlds.Values) world.Dispose();
//             Worlds.Clear();
//         }
//
//         public static AbstractWorld GetWorld(WorldIdentifier identifier)
//         {
//             if (Worlds.TryGetValue(identifier, out var world)) return world;
//             throw new Exception($"World {identifier} not found");
//         }
//
//         private IEnumerator ChunkGenerator()
//         {
//             var waiter = new WaitForSeconds(1 / 30f);
//             while (true)
//             {
//                 yield return waiter;
//                 for (var i = 0; i < Mathf.Min(MaxGenerationPerTick, ToGenerateChunksQueue.Count); i++)
//                     if (ToGenerateChunksQueue.TryDequeue(out var chunk))
//                         Task.Run(() => chunk.World.WorldGenerator.GenerateChunk(chunk));
//             }
//         }
//
//         public static void ChunksDispatcher()
//         {
//             while (true)
//             {
//                 Thread.Sleep(1000 / 30);
//                 for (var i = 0; i < Mathf.Min(MaxChunksPerTick, ChunkSenderQueue.Count); i++)
//                     if (ChunkSenderQueue.TryDequeue(out var chunk))
//                     {
//
//                         ChunkUtils.SendChunkFromThread(chunk);
//                     }
//             }
//         }
//
//         public static void GenerateChunk(Chunk chunk) => ToGenerateChunksQueue.Enqueue(chunk);
//         
//     }
// }

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MessagePack;
using Network.Messages;
using Network.Messages.Packets.World;
using Unity.Netcode;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utils;
using World.Blocks;
using World.Chunks;
using Task = System.Threading.Tasks.Task;

namespace World
{
    public class WorldsManager : MonoBehaviour
    {
        public static WorldsManager Instance { get; private set; }
        
        private readonly SemaphoreSlim _chunkGenerationSemaphore = new(Mathf.Max(1, Environment.ProcessorCount / 4)); 
        [SerializeField] public Tilemap worldTilemap;
        [SerializeField] public Tilemap backgroundTilemap;
        [SerializeField] public Tilemap vegetationTilemap;
        
        private readonly UniqueConcurrentQueue<Chunk> _chunksDispatcherQueue = new();
        private const float DispatchRate = 1 / 30f;
        private const int MaxDispatchPerTick = 30;
        
        private readonly Dictionary<WorldIdentifier, Queue<(Vector2Int, ChunkRequestType)>> _chunkRequestsQueue = new();
        private readonly Dictionary<WorldIdentifier, Dictionary<Vector2Int, float>> _chunksRequested = new();
        private const float RequestRate = 1 / 30f;
        private const int MaxRequestPerTick = 100;
        
        private readonly ConcurrentDictionary<WorldIdentifier, AbstractWorld> _worlds = new();
        private readonly Dictionary<WorldIdentifier, Type> _worldTypes = new();
        
        private readonly ConcurrentQueue<Chunk> _lightingQueue = new();
        private const int MaxLightingPerTick = 30;
        private void Awake()
        {
            Instance = this;
        }

        public void Init()
        {
            RegisterWorlds();
            StartCoroutine(ChunkDispatcher());
            StartCoroutine(ChunkRequester());
       //     StartCoroutine(LightingManager());
        }
        
        private void RegisterWorlds()
        {
            RegisterWorlds(WorldIdentifier.Overworld, typeof(Overworld));
        }

        private void RegisterWorlds(WorldIdentifier identifier, Type worldType) =>_worldTypes[identifier] = worldType;

        public void LoadWorlds()
        {
            foreach (var world in _worldTypes)
            {
                 var instance = (AbstractWorld)Activator.CreateInstance(world.Value, world.Key);
                 _worlds[world.Key] = instance;
                 _chunksRequested[world.Key] = new();
                 _chunkRequestsQueue[world.Key] = new();
            }
        }
       
        public AbstractWorld GetWorld(WorldIdentifier identifier)
        {
            if (_worlds.TryGetValue(identifier, out var world)) return world;
            throw new System.Exception($"World {identifier} not found");
        }


        public void EnqueueChunkRequest(Vector2 chunkPosition, WorldIdentifier worldIn,
            ChunkRequestType requestType = ChunkRequestType.Request)
        {
            var position = VectorUtils.GetNearestChunkPosition(chunkPosition);
            if (!_chunksRequested[worldIn].ContainsKey(position))
            {
                _chunkRequestsQueue[worldIn].Enqueue(
                    (VectorUtils.GetNearestChunkPosition(chunkPosition), requestType)
                );
                _chunksRequested[worldIn][position] = Time.realtimeSinceStartup;
            }
        } 
        
        private IEnumerator ChunkRequester()
        {
            while (true)
            {
                yield return new WaitForSeconds(RequestRate);
                foreach (WorldIdentifier worldIdentifier in Enum.GetValues(typeof(WorldIdentifier)))
                {
                    var queue = _chunkRequestsQueue[worldIdentifier];
                    var batchRequest = new List<Vector2Int>();
                    var batchDrop = new List<Vector2Int>();
                    for (int i = 0; i < Math.Min(MaxRequestPerTick, queue.Count); i++)
                    {
                        var request = queue.Dequeue();
                        var position = request.Item1;
                        if (request.Item2 == ChunkRequestType.Request)
                            batchRequest.Add(position);
                        else
                            batchDrop.Add(position);                        
                    }

                    if(batchRequest.Count > 0)
                        SendRequestPacket(batchRequest.ToArray(), ChunkRequestType.Request, worldIdentifier);
                    if (batchDrop.Count > 0)
                        SendRequestPacket(batchDrop.ToArray(), ChunkRequestType.Drop, worldIdentifier);
                }
            }
        }

        private void SendRequestPacket(Vector2Int[] batch, ChunkRequestType requestType, WorldIdentifier worldIn)
        {
            var packet = new ChunkRequestMessage
            {
                Positions = batch,
                Type = requestType,
                World = worldIn
            };
            MessageFactory.SendPacket(SendingMode.ClientToServer, packet);
        }

        public void EnqueueChunkToDispatch(Chunk chunk)
        {
            chunk.WorldIn.ServerHandler.Query.AddChunk(chunk);
            _chunksDispatcherQueue.Enqueue(chunk);
        }
        
        // private IEnumerator LightingManager()
        // {
        //     while (true)
        //     {
        //         yield return new WaitForSeconds(1 / 30f);
        //         for (var i = 0; i < Mathf.Min(MaxLightingPerTick, _lightingQueue.Count); i++)
        //             if (_lightingQueue.TryDequeue(out var chunk))
        //             {
        //                 var chunks = chunk.WorldIn.ServerHandler.Query.GetSurroundingChunks(chunk.Position);
        //                 if (chunks.Any(c => c == null))
        //                 {
        //                     _lightingQueue.Enqueue(chunk);
        //                     EnqueueChunkToDispatch(chunk);
        //                     continue;
        //                 }
        //
        //                 chunk.WorldIn.ServerHandler.LightingManager.UpdateLighting(chunk, chunks[3], chunks[2], chunks[1], chunks[0]);
        //                 EnqueueChunkToDispatch(chunk);
        //             }
        //     }
        // }
        private IEnumerator ChunkDispatcher()
        {
            while (true)
            {
                yield return new WaitForSeconds(DispatchRate);
                for (var i = 0; i < Mathf.Min(MaxDispatchPerTick, _chunksDispatcherQueue.Count); i++)
                    if (_chunksDispatcherQueue.TryDequeue(out var chunk))
                    {
                        SendChunkToClients(chunk.Players.ToArray(), chunk);
                    }
            }
        }
        
        private void SendChunkToClients(ulong[] clients, Chunk chunk)
        {
            if(clients.Length == 0) return;
            var buffer = chunk.Serialize();
            var packet = new ChunkTransferMessage
            {
                Position = chunk.Position,
                Data = buffer,
                World = chunk.WorldIn.Identifier
            };
            
            MessageFactory.SendPacket(SendingMode.ServerToClient, packet, clients, null, NetworkDelivery.ReliableFragmentedSequenced);
        }
        
        
        public void EnqueueChunkGeneration(AbstractWorld worldIn, Vector2Int position, ulong owner)
        {
            Task.Run(async () =>
            {
                await _chunkGenerationSemaphore.WaitAsync();
                try
                {
                    var chunk = new Chunk(worldIn, position);
                    await worldIn.ServerHandler.WorldGenerator.GenerateChunk(chunk);
                    chunk.AddPlayer(owner);
                    EnqueueChunkToDispatch(chunk);
                    _lightingQueue.Enqueue(chunk);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
                finally
                {
                    _chunkGenerationSemaphore.Release();
                }
            });
        }
    }
}