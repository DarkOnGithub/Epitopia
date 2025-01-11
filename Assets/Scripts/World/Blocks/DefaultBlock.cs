using MessagePack;
using Tiles;
using UnityEngine;

namespace World.Blocks
{
    [MessagePackObject]
    public struct DefaultBlockState : IBlockState
    {
        [Key(0)] public int Id { get; set; }
        [Key(1)] public int State { get; set; }
        [IgnoreMember] public string Name { get; set; }
        [IgnoreMember] public BlockProperties Properties { get; set; }
        [IgnoreMember] public IBlock Block { get; set; }
    }

    public class DefaultBlock : AbstractBlock<DefaultBlockState>
    {
        public DefaultBlock(int id, string name, BlockProperties properties) : base(id, name, properties)
        {
            if (properties.SpritePath != null)
                Tile = new BaseTile(Resources.Load<Texture2D>($"Sprites/Blocks/{properties.SpritePath}"), true, Name)
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