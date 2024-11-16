using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using World.Chunks;

namespace World
{
    public class WorldManager : MonoBehaviour
    {
        public static WorldManager Instance;
        public static Dictionary<WorldIdentifier, AbstractWorld> Worlds = new();
        
        [SerializeField] public Tilemap tilemap;

        [SerializeField] public Grid Grid;
        public void Awake()
        {
            RegisterWorlds();
            Instance = this;
        }
        public void RegisterWorlds()
        {
            RegisterWorld(new Overworld(WorldIdentifier.Overworld));
        } 
      
        public static void RegisterWorld(AbstractWorld world)
        {
            Worlds.Add(world.Identifier, world);
        }
        
        public static AbstractWorld GetWorld(WorldIdentifier identifier)
        {
            if (Worlds.TryGetValue(identifier, out var world))
                return world;
            throw new Exception($"World {identifier} not found");
        }
    }
}