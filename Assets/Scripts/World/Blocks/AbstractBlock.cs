using UnityEngine;
using UnityEngine.Tilemaps;
using Utils;

namespace World.Blocks
{
    public abstract class AbstractBlock<T> : IBlock
        where T : IBlockState
    {
        public int Id { get; }
        public string Name { get; }
        public BlockProperties Properties { get; }
        public Tile Tile { get; }
        public IBlockState IDefaultState { get { return DefaultState; } }
        public abstract T DefaultState { get; }
        
        public AbstractBlock(int id, string name, BlockProperties properties)
        {
            Id = id;
            Name = name;
            Properties = properties;
            Tile = SpriteUtils.GetTileFromSprite(Resources.Load<Sprite>(properties.SpritePath));
        }
        
        public abstract T GetState(object state);
        public abstract T GetDefaultState();
        
        IBlockState IBlock.GetDefaultState()
        {
            return GetDefaultState();
        }
        
        IBlockState IBlock.GetState(object state)
        {
            return GetState(state);
        }

        
        
    }
}