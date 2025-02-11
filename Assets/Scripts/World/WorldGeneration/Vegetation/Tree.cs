using UnityEngine;
using Utils;
using World.Blocks;
using World.Chunks;
using Random = System.Random;

namespace World.WorldGeneration.Vegetation
{
    public class Tree : IVegetationComponent
    {
        private AbstractBlock _block;
        private AbstractBlock _leavesBlock;
        private AbstractBlock _topBlock;
        public Tree(string name)
        {
            _leavesBlock = BlockRegistry.GetBlock(name + " Leaves");
            _block = BlockRegistry.GetBlock("TreeLog");
            _topBlock = BlockRegistry.GetBlock(name + " Top");
        }
        public bool CanGenerateAt(Chunk chunkIn, Vector2Int localPosition, Vector2Int origin,
            AbstractWorld worldIn)
        {
            var bottomBlock = (localPosition + Vector2Int.down);
            if (!VectorUtils.IsWithinChunkBounds(bottomBlock))
                return false;
            return  localPosition.x > 3
                    && localPosition.x < 13 
                    && localPosition.x % 4 == 0  
                    && chunkIn.GetBlockAt(bottomBlock.ToIndex()).Id != 0 && chunkIn.GetBlockAt(localPosition.ToIndex()).Id == 0;
        } 

        public void Generate(Chunk chunkIn, Vector2Int localPosition, Vector2Int origin, AbstractWorld world)
        {
            var random = new Random(localPosition.ToIndex() + Seed.SeedValue);
            var serverHandler = world.ServerHandler;
            var height = random.Next(5, 15);
            for(var i = 0; i < height; i++)
            {
                if (i > 2 && i < 15)
                {
                    foreach (var direction in new[]{new Vector2Int(1, 0), new Vector2Int(-1, 0)})
                    {
                        if(random.Next(0, 10) == 0)
                        {
                            var branchPosition = localPosition + new Vector2Int(0, i) + direction;
                            if (VectorUtils.IsWithinChunkBounds(branchPosition))
                                chunkIn.SetBlockAt(branchPosition.ToIndex(), _leavesBlock.CreateBlockState(0));
                            else
                                serverHandler.PlaceBlockFromWorldPosition(branchPosition + origin, _leavesBlock.CreateBlockState(0));
                        }
                    }        
                }
            
                var position = localPosition + new Vector2Int(0, i);
                if (VectorUtils.IsWithinChunkBounds(position))
                    chunkIn.SetBlockAt(position.ToIndex(), _block.CreateBlockState(0));
                else
                    serverHandler.PlaceBlockFromWorldPosition(position + origin, _block.CreateBlockState(0));
            }
            var topPosition = localPosition + new Vector2Int(0, height);
            var block = new Random().Next(0, 10) == 0 ? _block : _topBlock;
            if (VectorUtils.IsWithinChunkBounds(topPosition))
                chunkIn.SetBlockAt(topPosition.ToIndex(), block.CreateBlockState(0));
            else
                serverHandler.PlaceBlockFromWorldPosition(topPosition + origin, block.CreateBlockState(0));
        }
    }
}