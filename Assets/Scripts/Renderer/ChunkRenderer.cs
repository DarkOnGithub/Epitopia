using UnityEngine;
using UnityEngine.Tilemaps;
using Utils;
using World;
using World.Blocks;
using World.Chunks;

namespace Renderer
{
    public static class ChunkRenderer
    {
        private static Tilemap _tilemap = WorldManager.Instance.tilemap;

        public static void RenderChunk(Chunk chunk)
        {
            var (position, tiles) = ExtractTiles(chunk);
            _tilemap.SetTiles(position, tiles);
        }

        public static void ClearChunk(Chunk chunk)
        {
            var origin = chunk.Origin.ToVector3Int();

            var positions = new Vector3Int[Chunk.ChunkSizeSquared];
            for (int i = 0; i < Chunk.ChunkSizeSquared; i++)
                positions[i] = origin + i.ToVector3Int0();
            
            _tilemap.SetTiles(positions, null);
        }
        public static (Vector3Int[], Tile[]) ExtractTiles(Chunk chunk)
        {
            var position = new Vector3Int[Chunk.ChunkSizeSquared];
            var tiles = new Tile[Chunk.ChunkSizeSquared];
            var origin = chunk.Origin.ToVector3Int();
            var blocks = chunk.BlockStates;
            for (int i = 0; i < Chunk.ChunkSizeSquared; i++)
            {
                position[i] = origin + i.ToVector3Int0();
                tiles[i] = blocks[i].Block.Tile;
                if(blocks[i].Block.Tile == null)
                    Debug.Log("Tile is null");
            }

            return (position, tiles);
        }
        
        public static void UnRenderChunk(Chunk chunk)
        {
            var origin = chunk.Origin.ToVector3Int();
            var positions = new Vector3Int[Chunk.ChunkSizeSquared];
            for (int i = 0; i < Chunk.ChunkSizeSquared; i++)
                positions[i] = origin + i.ToVector3Int0();

    
            _tilemap.SetTiles(positions, new TileBase[Chunk.ChunkSizeSquared]);
        }
    }
}