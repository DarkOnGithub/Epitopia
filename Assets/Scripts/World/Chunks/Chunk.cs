﻿using System;
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

    public class Chunk : IDisposable
    {
        public const int ChunkSize = 16;
        public const int ChunkSizeSquared = ChunkSize * ChunkSize;
        public readonly AbstractWorld World;
        private bool _disposed;
        private bool _isRendered;

        public IBlockState[] BlockStates = new IBlockState[ChunkSizeSquared];
        public HashSet<ulong> Players = new();

        public Chunk(AbstractWorld worldIn, Vector2Int center)
        {
            World = worldIn ?? throw new ArgumentNullException(nameof(worldIn));
            UpdateContent(new IBlockState[ChunkSizeSquared]);
            Center = center;
        }

        public Chunk(AbstractWorld worldIn, Vector2Int center, IBlockState[] states)
        {
            World = worldIn ?? throw new ArgumentNullException(nameof(worldIn));
            UpdateContent(states);
            Center = center;
            IsEmpty = false;
        }

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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void UpdateContent(IBlockState[] newContent)
        {
            var air = BlockRegistry.BLOCK_AIR.GetDefaultState();
            for (var i = 0; i < ChunkSizeSquared; i++)
            {
                var newState = newContent[i];
                if (newState == null)
                    BlockStates[i] = air;
                else
                    BlockStates[i] = BlockRegistry.GetBlock(newState.Id).FromIBlockState(newState);
            }
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
        }

        public void RemoveBlock(int index)
        {
            ValidateIndex(index);
            BlockStates[index] = null;
        }

        public T GetBlockSafe<T>(int index) where T : IBlockState
        {
            return IsValidIndex(index) ? (T)BlockStates[index] : default;
        }

        public IBlockState GetBlockSafe(int index)
        {
            return IsValidIndex(index) ? BlockStates[index] : null;
        }

        public void SetBlockSafe(int index, IBlockState state)
        {
            if (IsValidIndex(index)) BlockStates[index] = state;
        }

        public void RemoveBlockSafe(int index)
        {
            if (IsValidIndex(index)) BlockStates[index] = null;
        }

        public ChunkData GetChunkData()
        {
            return new ChunkData
                   {
                       Center = Center,
                       BlockStates = BlockStates
                   };
        }


        private static bool IsValidIndex(int index)
        {
            return index >= 0 && index < ChunkSizeSquared;
        }

        private static void ValidateIndex(int index)
        {
            if (!IsValidIndex(index))
                throw new ArgumentOutOfRangeException(nameof(index),
                                                      $"Index must be between 0 and {ChunkSizeSquared - 1}");
        }

        public void Render()
        {
            if (_disposed) return;
            if (_isRendered) return;
            _isRendered = true;
            ChunkRenderer.RenderChunk(this);
        }

        public void UnRender()
        {
            ChunkRenderer.UnRenderChunk(this);
        }

        public void Destroy()
        {
            if (_isRendered)
                UnRender();
            World.ClientHandler.RemoveChunk(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            Destroy();

            if (disposing)
            {
                Players.Clear();
                BlockStates = null;
            }


            _disposed = true;
        }

        ~Chunk()
        {
            Dispose(false);
        }
    }
}