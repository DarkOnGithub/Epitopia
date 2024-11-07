namespace Blocks
{
    public struct DefaultBlockState : IBlockState
    {
        public int Id { get; set; }
        public AbstractBlock Block { get; set; }
        public BlockProperties Properties { get; set; }
        public int State;
    }
    public class DefaultBlock : AbstractBlock
    {
        public DefaultBlock(string name, BlockProperties properties) : base(name, properties)
        {
            
        }
        
        public DefaultBlockState CreateBlockData(int state = 0)
        {
            return new DefaultBlockState()
            {
                Properties = Properties,
                Block = this,
                Id = Id,
                State = state
            };
        }
    }
}