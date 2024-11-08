using System;
using System.Collections.Generic;
using UnityEngine;

namespace World
{
    public class WorldManager : MonoBehaviour
    {
        public static WorldManager Instance;
        public static Dictionary<WorldIdentifier, AbstractWorld> Worlds = new();
        public void Awake()
        {
            Instance = this;
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