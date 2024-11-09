using System;
using System.Collections.Generic;
using MessagePack;
using Renderer;
using UnityEngine;
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
        public List<ulong> Owners = new();
        public readonly AbstractWorld World;
        
        public Vector2Int Center { get; }
        public Vector2Int Origin { get; }
        public bool IsDrawn { get; private set; }
        public bool IsEmpty { get; set; }
        public ulong CurrentGenerator;

        public void UpdateContent(IBlockState[] states)
        {
            var air = BlockRegistry.BLOCK_AIR.GetDefaultState();
            for (int i = 0; i < ChunkSizeSquared; i++)
            {
                var state = states[i];
                if (state != null)
                    BlockStates[i] = BlockRegistry.GetBlock(state.Id).FromIBlockState(state);
                else
                    BlockStates[i] = air;
            }
        }
        public Chunk(AbstractWorld worldIn, Vector2Int center)
            : this(worldIn, center, new IBlockState[ChunkSizeSquared])
        {
            IsEmpty = true;
        }
        
        public Chunk(AbstractWorld worldIn, Vector2Int center, IBlockState[] states)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = $"Chunk {center}";
            go.transform.position = new Vector3(center.x, center.y);
            go.transform.localScale = new Vector3(ChunkSize, ChunkSize, 1);
            
            World = worldIn ?? throw new ArgumentNullException(nameof(worldIn));
            UpdateContent(states);
            
            Center = center;
            Origin = new Vector2Int(center.x - ChunkSize / 2, center.y - ChunkSize / 2);
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

        public void AddOwners(IEnumerable<ulong> clientIds)
        {
            Owners.AddRange(clientIds);
            OnOwnersUpdated();
        }
        
        public void AddOwner(ulong clientId)
        {
            Owners.Add(clientId);
            OnOwnersUpdated();
        }
        
        public void RemoveOwners(IEnumerable<ulong> clientIds)
        {
            foreach (var clientId in clientIds)
                Owners.Remove(clientId);
            OnOwnersUpdated();
        }
        
        public void RemoveOwner(ulong clientId)
        {
            Owners.Remove(clientId);
            OnOwnersUpdated();
        }
        
        public void TryDraw()
        {
            // if (IsDrawn) 
            //     return;
            IsDrawn = true;
            ChunkRenderer.RenderChunk(this);
        }

        public ChunkData GetChunkData() => new()
        {
            Center = Center,
            BlockStates = BlockStates
        };

        private void OnOwnersUpdated()
        {
            if (Owners.Count == 0)
                Destroy();
        }

        private static bool IsValidIndex(int index) 
            => index >= 0 && index < ChunkSizeSquared;

        private static void ValidateIndex(int index)
        {
            if (!IsValidIndex(index))
                throw new ArgumentOutOfRangeException(nameof(index), 
                    $"Index must be between 0 and {ChunkSizeSquared - 1}");
        }

        public void Destroy()
        {
            // Implementation needed
            throw new NotImplementedException();
        }
    }
}