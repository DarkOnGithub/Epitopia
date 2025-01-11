using UnityEngine.Tilemaps;

namespace World.Blocks
{
    public interface IBlock
    {
        public int Id { get; }
        public string Name { get; }
        public BlockProperties Properties { get; }
        public TileBase Tile { get; }
        public IBlockState IDefaultState { get; }

        public IBlockState GetDefaultState();
        public IBlockState GetState(object state);
        public IBlockState FromIBlockState(IBlockState state);
    }
}