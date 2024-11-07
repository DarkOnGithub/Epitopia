namespace Blocks
{
    public struct BlockContainer : IBlockData
    {
        public int BlockId { get; }
        public int[] BlockState { get; set; }
    }
}