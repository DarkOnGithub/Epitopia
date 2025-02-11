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

        public bool CanGenerateAt(Chunk chunkIn, Vector2Int localPosition, Vector2Int origin,
            AbstractWorld worldIn)
        {
            var bottomBlock = (localPosition + Vector2Int.down);
            if (!VectorUtils.IsWithinChunkBounds(bottomBlock))
                return false;
            return chunkIn.GetBlockAt(bottomBlock.ToIndex()).Id != 0 && chunkIn.GetBlockAt(localPosition.ToIndex()).Id == 0;
        } 

        public void Generate(Chunk chunkIn, Vector2Int localPosition, Vector2Int origin, AbstractWorld worldIn)
        {
            worldIn.ServerHandler.PlaceBlockFromWorldPosition(localPosition + origin + new Vector2Int(0, 100), _block.CreateBlockState() );
            chunkIn.SetBlockAt(localPosition.ToIndex(), _block.CreateBlockState());
        }
    }
}