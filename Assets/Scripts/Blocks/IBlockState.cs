namespace Blocks
{
    public interface IBlockState
    {
        public int Id { get; }
        
        public AbstractBlock Block { get; }
        public BlockProperties Properties { get; }
        
    }
}