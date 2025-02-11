using MessagePack;
using Tiles;

namespace World.Blocks.CustomBlocks
{

    public enum TreeNode
    {
        Log = 0,
        LeafLeft = 1,
        LeafRight = 2,
    }
    [MessagePackObject]
    public struct TreeBlockState :  IBlockState
    {
        [Key(0)]public int LightLevel { get; set; }
        [Key(1)]public byte WallId { get; set; }
        [Key(2)]public int Id { get; set; }
        [Key(3)]public TreeNode State { get; set; }
    }
    
    public class TreeBlock : AbstractBlock
    {
        public TreeBlock(int id, string name, BlockProperties properties) : base(id, name, properties)
        {
            Tile = new TreeLogTile(properties.SpritePath).RuleTile;   
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