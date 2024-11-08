using MessagePack;
using Unity.VisualScripting.Dependencies.Sqlite;

namespace Blocks
{
    [MessagePackObject]
    public struct DefaultBlockState : IBlockState
    {
        [Key(0)]
        public int Id { get; set; }
        [Key(1)]
        public int State;
        [IgnoreMember]
        public AbstractBlock Block { get; set; }
        [IgnoreMember]
        public BlockProperties Properties { get; set; }
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