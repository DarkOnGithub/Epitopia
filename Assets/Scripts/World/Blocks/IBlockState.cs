namespace World.Blocks
{
    public interface IBlockState
    {
        public int Id { get; }
        public string Name { get; }
        public BlockProperties Properties { get; }
        public IBlock Block { get; }
    }
}