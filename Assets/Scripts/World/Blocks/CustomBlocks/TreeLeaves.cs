using Tiles;

namespace World.Blocks.CustomBlocks
{
    public class TreeLeaves : AbstractBlock
    {
        public TreeLeaves(int id, string name, BlockProperties properties) : base(id, name, properties)
        {
            Tile = new TreeLeavesTile(properties.SpritePath).RuleTile;   
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