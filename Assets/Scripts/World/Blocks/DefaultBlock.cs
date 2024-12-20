using MessagePack;
using Unity.VisualScripting;

namespace World.Blocks
{
    [MessagePackObject]
    public struct DefaultBlockState : IBlockState
    {
        [Key(0)] public int Id { get; set; }
        [IgnoreMember] public string Name { get; set; }
        [IgnoreMember] public BlockProperties Properties { get; set; }
        [IgnoreMember] public IBlock Block { get; set; }
        [Key(1)] public int State { get; set; }
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

        public override DefaultBlockState FromState(DefaultBlockState state)
        {
            return GetState(state.State);
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

        public override DefaultBlockState GetDefaultState()
        {
            return DefaultState;
        }
    }
}