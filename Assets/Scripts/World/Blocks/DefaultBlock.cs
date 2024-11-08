using Unity.VisualScripting;

namespace World.Blocks
{
    public struct DefaultBlockState : IBlockState
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public BlockProperties Properties { get; set; }
        public IBlock Block { get; set; }
        public int State { get; set; }
    }
    
    public class DefaultBlock : AbstractBlock<DefaultBlockState>
    {
        
        public override DefaultBlockState DefaultState { get; }

        
        public DefaultBlock(int id, string name, BlockProperties properties) : base(id, name, properties)
        {
            DefaultState = new DefaultBlockState
            {
                Id = id,
                Name = name,
                Properties = properties,
                Block = this,
                State = 0
            };
        }

        public override DefaultBlockState GetState(object state)
        {
            if (state is not int cState)
                return DefaultState;
            return new DefaultBlockState()
                   {
                       Id = Id,
                       Name = Name,
                       Properties = Properties,
                       Block = this,
                       State = cState
                   };
        }

        public override DefaultBlockState GetDefaultState() => DefaultState;
    }
}