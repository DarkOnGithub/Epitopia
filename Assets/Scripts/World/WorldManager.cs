using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Network;
using Network.Messages;
using Network.Messages.Packets.World;
using UnityEngine;
using UnityEngine.Tilemaps;
using World.Chunks;
using Network.Server;
using TMPro;

namespace World
{
    public class WorldManager : MonoBehaviour
    {
        public static WorldManager Instance { get; private set; }
        public static ConcurrentDictionary<WorldIdentifier, AbstractWorld> Worlds { get; } = new();
        private static readonly Dictionary<WorldIdentifier, Type> _worlds = new();
        public static readonly ConcurrentQueue<Chunk> ChunkSenderQueue = new();
        private static readonly Queue<Chunk> ToGenerateChunksQueue = new();
        private const int MaxChunksPerTick = 20;
        private const int MaxGenerationPerTick = 20;
        
        [SerializeField] public Tilemap tilemap;
        [SerializeField] public Grid Grid;

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
                var instance = (AbstractWorld)Activator.CreateInstance(world.Value, new object[] { world.Key });
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
                        WorldGeneration.WorldGeneration.GenerateChunk(chunk.World, chunk);
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