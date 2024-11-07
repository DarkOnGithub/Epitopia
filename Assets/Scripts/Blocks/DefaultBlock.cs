namespace Blocks
{
    public struct DefaultBlock : IBlockData
    {
        public int BlockId { get; }
        public int BlockState { get; set; }
    }
}