// using System.Collections.Generic;
// using JetBrains.Annotations;
// using UnityEngine;
// using Utils;
// using World.Blocks;
// using World.Chunks;
//
// namespace World
// {
//     public class WorldQuery
//     {
//         private readonly AbstractWorld _worldIn;
//         public readonly Dictionary<Vector2Int, Chunk> Chunks;
//
//         public WorldQuery(AbstractWorld worldIn, Dictionary<Vector2Int, Chunk> chunks = null)
//         {
//             _worldIn = worldIn;
//             Chunks = chunks ?? new Dictionary<Vector2Int, Chunk>();
//         }
//
//         public void RemoveChunk(Vector2Int chunkPosition) => Chunks.Remove(chunkPosition);
//         
//
//         public void AddChunk(Chunk chunk) => Chunks.Add(chunk.Center, chunk);
//         
//
//         public void TryAddChunk(Chunk chunk)
//         {
//             if (!Chunks.TryGetValue(chunk.Center, out var _))
//                 Chunks.Add(chunk.Center, chunk);
//         }
//
//         public IEnumerable<Chunk> LazyGetChunks(Vector2Int[] chunkPositions)
//         {
//             foreach (var chunkPosition in chunkPositions)
//                 if (Chunks.TryGetValue(chunkPosition, out var chunk))
//                     yield return chunk;
//         }
//
//         [CanBeNull]
//         public Chunk GetChunk(Vector2Int chunkPosition)
//         {
//             return Chunks[chunkPosition];
//         }
//
//         public bool TryGetChunk(Vector2Int chunkPosition, out Chunk chunk)
//         {
//             return Chunks.TryGetValue(chunkPosition, out chunk);
//         }
//
//         public Chunk GetChunkOrCreate(Vector2Int chunkPosition)
//         {
//             if (!Chunks.TryGetValue(chunkPosition, out var chunk))
//                 return CreateEmptyChunk(chunkPosition);
//             return chunk;
//         }
//
//         public Chunk CreateEmptyChunk(Vector2Int chunkPosition)
//         {
//             var chunk = new Chunk(_worldIn, chunkPosition);
//             Chunks.Add(chunkPosition, chunk);
//             return chunk;
//         }
//
//         public Chunk CreateChunk(Vector2Int chunkPosition, IBlockState[] data)
//         {
//             var chunk = new Chunk(_worldIn, chunkPosition, data);
//             Chunks.Add(chunkPosition, chunk);
//             return chunk;
//         }
//
//         public bool FindNearestChunk(Vector2 worldPosition, out Chunk chunk)
//         {
//             var chunkPosition = WorldUtils.FindNearestChunkPosition(worldPosition);
//             return Chunks.TryGetValue(chunkPosition, out chunk);
//         }
//
//         public bool GetBlockFromWorldPosition(Vector2 position, out IBlockState blockState)
//         {
//             if (!FindNearestChunk(position, out var chunk))
//             {
//                 blockState = BlockRegistry.BLOCK_AIR.GetDefaultState();
//                 return false;
//             }
//             var localPosition = WorldUtils.WorldToLocalPosition(position, chunk.Origin);
//             blockState = chunk.GetBlock(localPosition.ToIndex());
//             return true;
//         }
//     }
// }

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using Utils;
using World.Chunks;

namespace World
{
    public class WorldQuery
    {
        private readonly ConcurrentDictionary<Vector2Int, Chunk> _chunks = new();
        private readonly AbstractWorld _worldIn;
        
        public WorldQuery(AbstractWorld worldIn)
        {
            _worldIn = worldIn;
        }
        
        public void RemoveChunk(Vector2Int chunkPosition) => _chunks.Remove(chunkPosition, out var _);
        public void AddChunk(Chunk chunk) => _chunks.TryAdd(chunk.Position, chunk);
        public bool HasChunk(Vector2Int chunkPosition) => _chunks.ContainsKey(chunkPosition);
        public bool TryGetChunk(Vector2Int worldPosition, out Chunk chunk) =>
            _chunks.TryGetValue(VectorUtils.GetNearestChunkPosition(worldPosition), out chunk);

        [CanBeNull]
        public Chunk GetChunk(Vector2Int chunkPosition) => _chunks.GetValueOrDefault(chunkPosition, null);
        
        public Chunk[] GetAllChunks() => _chunks.Values.ToArray();
        
        [ItemCanBeNull]
        public Chunk[] GetSurroundingChunks(Vector2Int position)
        {
            var chunks = new Chunk[4];
            var chunkPosition = VectorUtils.GetNearestChunkPosition(position);
            var x = chunkPosition.x;
            var y = chunkPosition.y;
            chunks[0] = GetChunk(new Vector2Int(x - Chunk.ChunkSize, y)); //left
            chunks[1] = GetChunk(new Vector2Int(x + Chunk.ChunkSize, y)); //right
            chunks[2] = GetChunk(new Vector2Int(x, y - Chunk.ChunkSize)); //bottom
            chunks[3] = GetChunk(new Vector2Int(x, y + Chunk.ChunkSize)); //top
            return chunks;
        }
        [ItemCanBeNull]
        public IEnumerable<Chunk> GetChunks(Vector2Int[] chunkPositions)
        {
            foreach (var position in chunkPositions)
            {
                if (_chunks.TryGetValue(position, out var chunk))
                    yield return chunk;
                yield return null;
            }
        }

        public void Destroy()
        {
            _chunks.Clear();
        }
        
        
    }    
}