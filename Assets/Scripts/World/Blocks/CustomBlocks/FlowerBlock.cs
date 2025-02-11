using Tiles;
using UnityEngine;

namespace World.Blocks.CustomBlocks
{
    public class FlowerBlock : AbstractBlock

    {
        public FlowerBlock(int id, string name, BlockProperties properties) : base(id, name, properties)
        {
            BlockRegistry.VegetationBlocks.TryAdd(BlockId, 0);
            Tile = new FlowerTile(properties.SpritePath).RuleTile;
        }

        public override IBlockState CreateBlockState(int? state = null, byte wallId = 0, int lightLevel = 0)
        {
            return new DefaultBlockState()
                   {
                       Id = BlockId,
                       WallId = wallId,
                       LightLevel = lightLevel
                   };
        }
    }
}