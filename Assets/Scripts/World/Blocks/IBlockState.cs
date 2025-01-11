using MessagePack;

namespace World.Blocks
{
    [Union(0, typeof(DefaultBlockState))]
    [Union(1, typeof(Wall))]
    public interface IBlockState
    {
        public int Id { get; }
        public string Name { get; }
        public BlockProperties Properties { get; }
        public IBlock Block { get; }
    }
}