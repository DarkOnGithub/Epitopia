using MessagePack;
using Tiles;
using UnityEngine;

namespace World.Blocks
{
    [MessagePackObject]
    public struct Wall : IBlockState
    {
        [Key(0)] public int Id { get; set; }
        [IgnoreMember] public string Name { get; set; }
        [IgnoreMember] public BlockProperties Properties { get; set; }
        [IgnoreMember] public IBlock Block { get; set; }
    }

    public class BaseWall : AbstractBlock<Wall>
    {
        public BaseWall(int id, string name, BlockProperties properties) : base(id, name, properties)
        {
            Tile = new WallTile(Resources.Load<Texture2D>($"Sprites/Blocks/{properties.SpritePath}")).RuleTile;
            DefaultState = new Wall
                           {
                               Id = id,
                               Name = name,
                               Properties = properties,
                               Block = this
                           };
            DefaultRuleTile.Walls.Add(Tile);
        }

        public override Wall DefaultState { get; }

        public override Wall GetState(object state)
        {
            return DefaultState;
        }

        public override Wall GetDefaultState()
        {
            return DefaultState;
        }

        public override Wall FromState(Wall state)
        {
            return DefaultState;
        }
    }
}