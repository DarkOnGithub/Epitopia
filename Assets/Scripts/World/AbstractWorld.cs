using System.Collections.Generic;
using Core;
using UnityEngine;
using World.Chunks;

namespace World
{
    public abstract partial class AbstractWorld
    {
        protected BetterLogger Logger = new BetterLogger(typeof(AbstractWorld));
        
        public Dictionary<Vector2Int, Chunk> Chunks = new();
        public Dictionary<Vector2Int, Chunk> HostChunks = new();
        public WorldIdentifier Identifier;

        public AbstractWorld(WorldIdentifier identifier)
        {
            Identifier = identifier;
        }

        
        public bool GetChunk(Vector2Int center, out Chunk chunk)
        {
            chunk = Chunks.GetValueOrDefault(center, null);
            return chunk == null;
        }

   
        public bool ContainsChunk(Vector2Int center)
        {
            return Chunks.ContainsKey(center);
        }
        
        public Chunk CreateChunk(Vector2Int center)
        {
            if(ContainsChunk(center))
            {
                Logger.LogWarning($"Chunk at {center} already exists");
                return null;
            }
            var chunk = new Chunk(center);
            Chunks.TryAdd(center, chunk);
            return chunk;
        }

        public Chunk CreateChunk(Vector2Int center, ChunkData data)
        {
            if(ContainsChunk(center))
            {
                Logger.LogWarning($"Chunk at {center} already exists");
                return null;
            }
            var chunk = new Chunk(center, data.BlockStates);
            Chunks.TryAdd(center, chunk);
            return chunk;

        }

    }
}