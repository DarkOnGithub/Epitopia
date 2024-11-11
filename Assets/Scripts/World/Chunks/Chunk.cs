using System;
using System.Collections.Generic;
using MessagePack;
using Renderer;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utils;
using World.Blocks;

namespace World.Chunks
{
    [MessagePackObject]
    public struct ChunkData
    {
        [Key(0)] public Vector2Int Center { get; set; }
        [Key(1)] public IBlockState[] BlockStates { get; set; }
    }
    
    public class Chunk
    {
        public const int ChunkSize = 16;
        public const int ChunkSizeSquared = ChunkSize * ChunkSize;
        
        public IBlockState[] BlockStates = new IBlockState[ChunkSizeSquared];
        public HashSet<ulong> Players = new();
        public readonly AbstractWorld World;
        private bool _isRendered;
        
        public Vector2Int Center { get; }

        public Vector2Int Origin
        {
            get
            {
                var half = ChunkSize / 2;
                return new Vector2Int(Center.x - half, Center.y - half);   
            }
        }

        public bool IsEmpty { get; set; } = true;

        public void UpdateContent(IBlockState[] newContent)
        {
            var air = BlockRegistry.BLOCK_AIR.GetDefaultState();
            for (int i = 0; i < ChunkSizeSquared; i++)
            {
                var newState = newContent[i];
                BlockStates[i] = BlockRegistry.GetBlock(newState.Id).FromIBlockState(newState) ?? air;
            }
        }
        public Chunk(AbstractWorld worldIn, Vector2Int center)
            : this(worldIn, center, new IBlockState[ChunkSizeSquared])
        {
            
        }
        
        public Chunk(AbstractWorld worldIn, Vector2Int center, IBlockState[] states)
        {
            World = worldIn ?? throw new ArgumentNullException(nameof(worldIn));
            UpdateContent(states);
            Center = center;
            IsEmpty = false;
        }

        public T GetBlock<T>(int index) where T : IBlockState
        {
            ValidateIndex(index);
            return (T)BlockStates[index];
        }

        public IBlockState GetBlock(int index)
        {
            ValidateIndex(index);
            return BlockStates[index];
        }

        public void SetBlock(int index, IBlockState state)
        {
            ValidateIndex(index);
            BlockStates[index] = state;
            World.OnChunkUpdated(this);
        }

        public void RemoveBlock(int index)
        {
            ValidateIndex(index);
            BlockStates[index] = null;
            World.OnChunkUpdated(this);
        }
        
        public T GetBlockSafe<T>(int index) where T : IBlockState
            => IsValidIndex(index) ? (T)BlockStates[index] : default;
        
        public IBlockState GetBlockSafe(int index)
            => IsValidIndex(index) ? BlockStates[index] : null;
        
        public void SetBlockSafe(int index, IBlockState state)
        {
            if (IsValidIndex(index))
            {
                BlockStates[index] = state;
                World.OnChunkUpdated(this);
            }
        }
        
        public void RemoveBlockSafe(int index)
        {
            if (IsValidIndex(index))
            {
                BlockStates[index] = null;
                World.OnChunkUpdated(this);
            }
        }

      


        public ChunkData GetChunkData() => new()
        {
            Center = Center,
            BlockStates = BlockStates
        };
        

        private static bool IsValidIndex(int index) 
            => index >= 0 && index < ChunkSizeSquared;

        private static void ValidateIndex(int index)
        {
            if (!IsValidIndex(index))
                throw new ArgumentOutOfRangeException(nameof(index), 
                    $"Index must be between 0 and {ChunkSizeSquared - 1}");
        }

        public void Render()
        {
            
        }
        
        public void UnRender()
        {
            
        }
        public void Destroy()
        {
           World.Query.RemoveChunk(Center);
           if(_isRendered)
                UnRender();    
        }
    }
}