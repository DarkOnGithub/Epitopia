using System.Collections.Generic;
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
        public enum TilemapType
        {
            World,
            Background
        }

        private static readonly Tilemap _worldTilemap = WorldManager.Instance.worldTilemap;
        private static readonly Tilemap _backgroundTilemap = WorldManager.Instance.backgroundTilemap;

        public static void RenderChunk(Chunk chunk)
        {
            var tiles = ExtractTiles(chunk);

            SetAndRefreshTiles(_worldTilemap, tiles[TilemapType.World]);
            SetAndRefreshTiles(_backgroundTilemap, tiles[TilemapType.Background]);
        }

        private static void SetAndRefreshTiles(Tilemap tilemap, (Vector3Int[], TileBase[]) tileData)
        {
            tilemap.SetTiles(tileData.Item1, tileData.Item2);
            foreach (var pos in tileData.Item1)
                tilemap.RefreshTile(pos);
        }

        public static Dictionary<TilemapType, (Vector3Int[], TileBase[])> ExtractTiles(Chunk chunk)
        {
            var worldPositions = new Vector3Int[Chunk.ChunkSizeSquared];
            var worldTiles = new TileBase[Chunk.ChunkSizeSquared];
            var backgroundPositions = new Vector3Int[Chunk.ChunkSizeSquared];
            var backgroundTiles = new TileBase[Chunk.ChunkSizeSquared];
            var origin = chunk.Origin.ToVector3Int();
            var blocks = chunk.BlockStates;
            for (var i = 0; i < Chunk.ChunkSizeSquared; i++)
            {
                var block = blocks[i];
                if (BlockRegistry.WallIds.Contains(block.Id))
                {
                    backgroundPositions[i] = origin + i.ToVector3Int0();
                    backgroundTiles[i] = block.Block.Tile;
                }
                else
                {
                    worldPositions[i] = origin + i.ToVector3Int0();
                    worldTiles[i] = block.Block.Tile;
                }
            }

            return new Dictionary<TilemapType, (Vector3Int[], TileBase[])>
                   {
                       { TilemapType.World, (worldPositions, worldTiles) },
                       { TilemapType.Background, (backgroundPositions, backgroundTiles) }
                   };
        }

        public static void UnRenderChunk(Chunk chunk)
        {
            var origin = chunk.Origin.ToVector3Int();
            var positions = new Vector3Int[Chunk.ChunkSizeSquared];
            for (var i = 0; i < Chunk.ChunkSizeSquared; i++)
                positions[i] = origin + i.ToVector3Int0();


            _worldTilemap.SetTiles(positions, new TileBase[Chunk.ChunkSizeSquared]);
        }
    }
}