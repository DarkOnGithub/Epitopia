using System.Collections.Generic;
using PimDeWitte.UnityMainThreadDispatcher;
using UnityEngine;
using Utils;
using World.Blocks;
using World.Chunks;
using Random = System.Random;

namespace World.WorldGeneration.Vegetation
{
    public class Tree : IVegetationComponent
    {
        private Dictionary<Vector2Int, int> _heightMap = new();
        public bool CanGenerateAt(IBlockState[] chunkIn, Vector2Int localPosition, Vector2Int origin)
        {
            var height = new Random().Next(5, 10);
            for (var i = localPosition.y; i < Mathf.Clamp(localPosition.y + height, localPosition.y, Chunk.ChunkSize); i++)
            {
                int index = localPosition.x + i * Chunk.ChunkSize;
                if (chunkIn[index].Id != 0)
                    return false;
            }
            _heightMap.Add(localPosition, height);
            return true;
        }

        public void Generate(IBlockState[] chunkIn, Vector2Int localPosition, Vector2Int origin)
        {
            chunkIn[localPosition.ToIndex()] = BlockRegistry.GetBlock("TreeBlock").GetState(_heightMap[localPosition]);
        }
    }
}