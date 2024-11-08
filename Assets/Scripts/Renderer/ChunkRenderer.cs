using Network.Messages.Packets.World;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utils;
using World.Chunks;

namespace Renderer
{
    public static class ChunkRenderer
    {
        public static Tilemap Tilemap = World.World.Tilemap;

        public static void RenderChunk(Chunk chunk)
        {
            var (position, tiles) = ExtractTiles(chunk);
            Tilemap.SetTiles(position, tiles);
        }

        public static (Vector3Int[], Tile[]) ExtractTiles(Chunk chunk)
        {
            var position = new Vector3Int[AbstractChunk.ChunkSizeSquared];
            var tiles = new Tile[AbstractChunk.ChunkSizeSquared];
            var origin = chunk.Origin.ToVector3Int();
            var blocks = chunk.Blocks;
            for (int i = 0; i < AbstractChunk.ChunkSizeSquared; i++)
            {
                position[i] = origin + i.ToVector3Int0();
                tiles[i] = blocks[i].Block.Tile;
            }

            return (position, tiles);
        }
    }
}