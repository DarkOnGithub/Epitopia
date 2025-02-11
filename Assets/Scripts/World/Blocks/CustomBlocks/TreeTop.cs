using Tiles;

namespace World.Blocks.CustomBlocks
{
    public class TreeTop : AbstractBlock
    {
        public TreeTop(int id, string name, BlockProperties properties) : base(id, name, properties)
        {
            Tile = new TreeTopTile(properties.SpritePath).RuleTile;
        }

        public override IBlockState CreateBlockState(int? state = null, byte wallId = 0, int lightLevel = 0)
        {
            return new TreeBlockState
                   {
                       Id = BlockId,
                       WallId = wallId,
                       LightLevel = lightLevel,
                       State = (TreeNode) (state ?? 0)
                   };     
        }
    }
}