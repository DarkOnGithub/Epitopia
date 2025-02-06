using UnityEngine;
using Utils;
using World.Blocks;
using World.Chunks;

namespace World.WorldGeneration.Vegetation
{
    public class Flower : IVegetationComponent
    {
        private AbstractBlock _block;
        
        public Flower(AbstractBlock block)
        {
            _block = block;
        }
        
        public bool CanGenerateAt(IBlockState[] chunkIn, Vector2Int localPosition, Vector2Int origin) => chunkIn[localPosition.ToIndex()].Id == 0;

        public void Generate(IBlockState[] chunkIn, Vector2Int localPosition, Vector2Int origin) => chunkIn[localPosition.ToIndex()] = _block.CreateBlockState();
        
        
    }
}