using MessagePack;

namespace World.Blocks
{
    public interface IBlockState
    {
        //4 bytes RGB-Level
        public int LightLevel { get; set; }
        public byte WallId { get; set; }
        public int Id { get;  } 
    }
}