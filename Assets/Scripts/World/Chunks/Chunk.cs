using System.Collections.Generic;
using MessagePack;
using UnityEngine;
using World.Blocks;

namespace World.Chunks
{
    [MessagePackObject]
    public struct ChunkData
    {
        [Key(0)] public Vector2Int Center { get; set; }

        [Key(1)]
        public IBlockState[] BlockStates;
    }
    
    public class Chunk
    {
        public const int ChunkSize = 16;
        public const int ChunkSizeSquared = ChunkSize * ChunkSize;
        
        public readonly IBlockState[] BlockStates = new IBlockState[ChunkSizeSquared];

        public Vector2Int Center;
        public Vector2Int Origin;

        public List<ulong> Owners = new();
     
        public bool IsEmpty = true;
        public Chunk(Vector2Int center)
        {
            Center = center;
            Origin = new Vector2Int(center.x - ChunkSize / 2, center.y - ChunkSize / 2);
        }
        
        public Chunk(Vector2Int center, IBlockState[] blockStates)
        {
            Center = center;
            Origin = new Vector2Int(center.x - ChunkSize / 2, center.y - ChunkSize / 2);
            BlockStates = blockStates;
            IsEmpty = false;
        }

        public T GetBlock<T>(int index) => (T) BlockStates[index];

        public IBlockState GetBlock(int index) => BlockStates[index];

        public void SetBlock(int index, IBlockState blockState) => BlockStates[index] = blockState;

        public void RemoveBlock(int index) => BlockStates[index] = null;
        
        public ChunkData GetChunkData()
        {
            return new ChunkData()
                   {
                       Center = Center,
                       BlockStates = BlockStates
                   };
        }

        public void Destroy()
        {
            throw new System.NotImplementedException();
        }
    }
}