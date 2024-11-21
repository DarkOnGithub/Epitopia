using System;
using System.Collections;
using System.Collections.Generic;
using Network;
using UnityEngine;
using UnityEngine.Tilemaps;
using World.Chunks;

namespace World
{
    public class WorldManager : MonoBehaviour
    {
        public static WorldManager Instance;
        public static Dictionary<WorldIdentifier, AbstractWorld> Worlds = new();
        private static Dictionary<WorldIdentifier, Type> _worlds = new();
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


        public static void LoadWorlds(Server server)
        {
            
            foreach (var world in _worlds)
            {
                var instance = (AbstractWorld) Activator.CreateInstance(world.Value);
                Worlds.Add(world.Key, instance);
            }
        }
        
        public static void UnloadWorlds()
        {
            foreach (var world in Worlds)
            {
                world.Value.Dispose();
            }
            Worlds.Clear();
        }
        public static AbstractWorld GetWorld(WorldIdentifier identifier)
        {
            if (Worlds.TryGetValue(identifier, out var world))
                return world;
            throw new Exception($"World {identifier} not found");
        }
    }
}