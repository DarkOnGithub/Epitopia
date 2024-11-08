using MessagePack;
using Unity.VisualScripting.Dependencies.Sqlite;

namespace Blocks
{
    [MessagePack.Union(0, typeof(DefaultBlockState))]
    public interface IBlockState
    {
        public int Id { get; }
        [IgnoreMember]
        public AbstractBlock Block { get; }
        [IgnoreMember]
        public BlockProperties Properties { get; }
        
    }
}