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
        public static WorldManager Instance;
        public static ConcurrentDictionary<WorldIdentifier, AbstractWorld> Worlds = new();
        private static Dictionary<WorldIdentifier, Type> _worlds = new();
        public static readonly ConcurrentDictionary<ulong, ConcurrentBag<Chunk>> ChunkSenderQueue = new();      
        private static readonly Queue<Chunk> GenerateChunksQueue = new();
        private const int MaxChunksPerTick = 20;
        [SerializeField] public Tilemap tilemap;

        [SerializeField] public Grid Grid;

        public void Awake()
        {
            RegisterWorlds();
            Instance = this;
        }

        public void RegisterWorlds()
        {
            RegisterWorld(typeof(Overworld), WorldIdentifier.Overworld);
        }

        public static void RegisterWorld(Type world, WorldIdentifier identifier)
        {
            _worlds.Add(identifier, world);
        }


        public static void LoadWorlds()
        {
            UnloadWorlds();
            foreach (var world in _worlds)
            {
                var instance = (AbstractWorld)Activator.CreateInstance(world.Value, new object[] { world.Key });
                ChunkSenderQueue.TryAdd(instance, new List<Chunk>());
                Worlds.TryAdd(world.Key, instance);
            }
        }

    
        public static void UnloadWorlds()
        {
            foreach (var world in Worlds)
                world.Value.Dispose();

            Worlds.Clear();
        }

        public static AbstractWorld GetWorld(WorldIdentifier identifier)
        {
            if (Worlds.TryGetValue(identifier, out var world))
                return world;
            throw new Exception($"World {identifier} not found");
        }

        public static void StartWorldThread()
        {
            while (true)
            {
                Thread.Sleep(100 / 3);
                for (int i = 0; i < Mathf.Min(MaxChunksPerTick, GenerateChunksQueue.Count); i++)
                {
                    if (GenerateChunksQueue.TryDequeue(out var chunk))
                    {
                        //GENERATE
                    }
                }
            }
        }

        private IEnumerable SendChunksPeriodically()
        {
            while (true)
            {
                Thread.Sleep(100 / 3);
                
                if (ChunkSenderQueue.TryDequeue(out var chunk))
                {
                    
                }
            }
        }
        public static void GenerateChunk(Chunk chunk)
        {
            GenerateChunksQueue.Enqueue(chunk);
        }
    }
}