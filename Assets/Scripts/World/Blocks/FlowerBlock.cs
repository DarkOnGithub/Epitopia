using Tiles;

namespace World.Blocks
{
    public class FlowerBlock : AbstractBlock<DefaultBlockState>
    {
        public FlowerBlock(int id, string name, BlockProperties properties) : base(id, name, properties)
        {
            if (properties.SpritePath != null)
                Tile = new FlowerTile("Sprites/Blocks/" + properties.SpritePath)
                   .RuleTile;


            DefaultState = new DefaultBlockState
                           {
                               Id = id,
                               Name = name,
                               Properties = properties,
                               Block = this
                           };
        }

        public override DefaultBlockState DefaultState { get; }

        public override DefaultBlockState FromState(DefaultBlockState state) => GetState(null);
        

        public override DefaultBlockState GetState(object state) => DefaultState;
        

        public override DefaultBlockState GetDefaultState() => DefaultState;
    }
}