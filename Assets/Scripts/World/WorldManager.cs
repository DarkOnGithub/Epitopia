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

namespace World
{
    public class WorldManager : MonoBehaviour
    {
        public static WorldManager Instance;
        public static ConcurrentDictionary<WorldIdentifier, AbstractWorld> Worlds = new();
        private static Dictionary<WorldIdentifier, Type> _worlds = new();
        public static readonly ConcurrentQueue<(NetworkUtils.Header header, ChunkTransferMessage message)> PacketQueue = new();
        private const int MaxItemsPerTick = 40;
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
                var instance = (AbstractWorld) Activator.CreateInstance(world.Value, new object[] { world.Key });
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
        
        public static void StartWorldTread()
        {
            while (true)
            {
                Thread.Sleep(100 / 3);
                int itemsProcessed = 0;
                while (itemsProcessed < MaxItemsPerTick && PacketQueue.TryDequeue(out var result))
                {
                    var handler = GetWorld(result.message.World).ServerHandler;
                    handler.OnPacketReceived(result.header, result.message);
                    itemsProcessed++;
                }
            }
        }
    }
}