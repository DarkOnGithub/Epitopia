using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Utils;
using World.Blocks;
using World.Chunks;
using Random = System.Random;

namespace World.WorldGeneration.Structures
{
    public struct FlowerVegetationData
    {
        public int[] Range;
        public float Probability;
        public string BlockName;
        public int MaxSize;
    }
    
    public class FlowerVegetation : IStructure
    {
        public IBlock Block;
        private int MaxSize;
        public float Probability { get; }
        public Func<float, bool> IsWithinRange;        
        
        public FlowerVegetation(FlowerVegetationData data)
        {
            var range = data.Range;
            if (range[0] == range[1])
                IsWithinRange = point => Mathf.Abs(point - range[0]) < 0.01f;
            else
                IsWithinRange = point => point >= range[0] && point <= range[1];
            
            Block = BlockRegistry.GetBlock(data.BlockName);
            MaxSize = data.MaxSize;
        }

        public Vector2Int GetBounds()
        {
            return new Vector2Int(1, 1);
        }

        [CanBeNull]
        public Dictionary<Vector2Int, (IBlockState, bool)> TryPlace(Vector2 position, float point)
        {
            var randomizer = new Random((int)(position.x) + Seed.SeedValue);
            if (!IsWithinRange(point) || randomizer.NextDouble() <= Probability)
                return null;
            
            var size = randomizer.Next(0, MaxSize);

            var x = (int)position.x;
            var y = (int)position.y;
            var blocksBuffer = new Dictionary<Vector2Int, (IBlockState, bool)>();

            
            for(int i = 0; i < size; i++)
                blocksBuffer[new Vector2Int(x + i, y)] = (Block.GetDefaultState(), false);
            return blocksBuffer;
        }
    }
}