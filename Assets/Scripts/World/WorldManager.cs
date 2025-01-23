using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;
using World.Chunks;

namespace World
{
    public class WorldManager : MonoBehaviour
    {
        private const int MaxChunksPerTick = 20;
        private const int MaxGenerationPerTick = 20;
        private static readonly Dictionary<WorldIdentifier, Type> _worlds = new();
        public static readonly ConcurrentQueue<Chunk> ChunkSenderQueue = new();
        private static readonly Queue<Chunk> ToGenerateChunksQueue = new();

        [SerializeField] public Tilemap worldTilemap;
        [SerializeField] public Tilemap backgroundTilemap;

        [SerializeField] public Grid Grid;
        public static WorldManager Instance { get; private set; }
        public static ConcurrentDictionary<WorldIdentifier, AbstractWorld> Worlds { get; } = new();

        private void Awake()
        {
            RegisterWorlds();
            Instance = this;
            StartCoroutine(ChunkGenerator());
        }

        private void RegisterWorlds()
        {
            RegisterWorld(typeof(Overworld), WorldIdentifier.Overworld);
        }

        public static void RegisterWorld(Type world, WorldIdentifier identifier)
        {
            _worlds[identifier] = world;
        }

        public static void LoadWorlds()
        {
            UnloadWorlds();
            foreach (var world in _worlds)
            {
                var instance = (AbstractWorld)Activator.CreateInstance(world.Value, world.Key);
                Worlds[world.Key] = instance;
            }
        }

        public static void UnloadWorlds()
        {
            foreach (var world in Worlds.Values) world.Dispose();
            Worlds.Clear();
        }

        public static AbstractWorld GetWorld(WorldIdentifier identifier)
        {
            if (Worlds.TryGetValue(identifier, out var world)) return world;
            throw new Exception($"World {identifier} not found");
        }

        private IEnumerator ChunkGenerator()
        {
            var waiter = new WaitForSeconds(1 / 30f);
            while (true)
            {
                yield return waiter;
                for (var i = 0; i < Mathf.Min(MaxGenerationPerTick, ToGenerateChunksQueue.Count); i++)
                    if (ToGenerateChunksQueue.TryDequeue(out var chunk))
                        Task.Run(() => chunk.World.WorldGenerator.GenerateChunk(chunk));
            }
        }

        public static void ChunksDispatcher()
        {
            while (true)
            {
                Thread.Sleep(1000 / 30);
                for (var i = 0; i < Mathf.Min(MaxChunksPerTick, ChunkSenderQueue.Count); i++)
                    if (ChunkSenderQueue.TryDequeue(out var chunk))
                        ChunkUtils.SendChunkFromThread(chunk);
            }
        }

        public static void GenerateChunk(Chunk chunk)
        {
            
            ToGenerateChunksQueue.Enqueue(chunk);
        }
    }
}