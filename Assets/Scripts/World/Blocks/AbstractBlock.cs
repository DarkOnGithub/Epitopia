// using UnityEngine.Tilemaps;
//
// namespace World.Blocks
// {
//     public abstract class AbstractBlock<T> : IBlock
//         where T : IBlockState
//     {
//         public AbstractBlock(int id, string name, BlockProperties properties)
//         {
//             Id = id;
//             Name = name;
//             Properties = properties;
//         }
//
//         public abstract T DefaultState { get; }
//         public int Id { get; }
//         public string Name { get; }
//         public BlockProperties Properties { get; }
//         public TileBase Tile { get; set; }
//         public IBlockState IDefaultState => DefaultState;
//
//         IBlockState IBlock.GetDefaultState()
//         {
//             return GetDefaultState();
//         }
//
//         IBlockState IBlock.GetState(object state)
//         {
//             return GetState(state);
//         }
//
//         IBlockState IBlock.FromIBlockState(IBlockState state)
//         {
//             return FromState((T)state);
//         }
//
//         public abstract T GetState(object state);
//         public abstract T GetDefaultState();
//         public abstract T FromState(T state);
//     }
// }

using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Tilemaps;
using World.Chunks;

namespace World.Blocks
{
    public abstract class AbstractBlock
    {
        public BlockProperties Properties;
        public int BlockId;
        public string BlockName;
        public TileBase Tile;
        
        protected AbstractBlock(int id, string name, BlockProperties properties)
        {
            BlockId = id;
            BlockName = name;
            Properties = properties;
        }

        public abstract IBlockState CreateBlockState(int? state = null, byte wallId = 0, int lightLevel = 0);
        public virtual void OnPlace(Chunk chunkIn, Vector2Int localPosition, IBlockState state) { }
        public virtual void OnBreak(Chunk chunkIn, Vector2Int localPosition, IBlockState state) { }
    } 
}