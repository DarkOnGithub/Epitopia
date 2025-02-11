using Tiles;

namespace World.Blocks.CustomBlocks
{
    public class GrassBlock : AbstractBlock
    {
        public GrassBlock(int id, string name, BlockProperties properties) : base(id, name, properties)
        {
            Tile = new DefaultTile(properties.SpritePath).RuleTile;
        }

        public override IBlockState CreateBlockState(int? state = null, byte wallId = 0, int lightLevel = 0)
        {
            return new DefaultBlockState
                   {
                       Id = BlockId,
                       LightLevel = lightLevel,
                       WallId = wallId
                   };
        }
    }
}