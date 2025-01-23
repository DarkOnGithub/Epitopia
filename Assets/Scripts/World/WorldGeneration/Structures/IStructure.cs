using System.Collections.Generic;
using UnityEngine;
using World.Blocks;

namespace World.WorldGeneration.Structures
{
    public interface IStructure
    {
        public float Probability { get; }
        public Vector2Int GetBounds();
        public Dictionary<Vector2Int, (IBlockState, bool)> TryPlace(Vector2 position, float point);
    }
}