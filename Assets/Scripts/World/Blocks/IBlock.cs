using UnityEngine.Tilemaps;

namespace World.Blocks
{
    public interface IBlock
    {
        public int Id { get; }
        public string Name { get; }
        public BlockProperties Properties { get; }
        public Tile Tile { get; }
        public IBlockState IDefaultState { get; }
        
        public IBlockState GetDefaultState();
        public IBlockState GetState(object state);
    }
}