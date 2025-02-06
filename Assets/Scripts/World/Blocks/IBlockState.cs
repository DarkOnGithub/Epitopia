using MessagePack;
using World.Blocks.CustomBlocks;

namespace World.Blocks
{
    [Union(0, typeof(DefaultBlockState))]
    public interface IBlockState
    {
        //4 bytes RGB-Level
        public int LightLevel { get; set; }
        public byte WallId { get; set; }
        public int Id { get;  } 
    }
}