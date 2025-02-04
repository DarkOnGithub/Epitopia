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
            Background,
            Vegetation
        }

        private static readonly Tilemap WorldTilemap = WorldManager.Instance.worldTilemap;
        private static readonly Tilemap BackgroundTilemap = WorldManager.Instance.backgroundTilemap;
        private static readonly Tilemap VegetationTilemap = WorldManager.Instance.vegetationTilemap;

        public static void RenderChunk(Chunk chunk)
        {
            var tiles = ExtractTiles(chunk);

            SetAndRefreshTiles(WorldTilemap, tiles[TilemapType.World]);
            SetAndRefreshTiles(BackgroundTilemap, tiles[TilemapType.Background]);
            SetAndRefreshTiles(VegetationTilemap, tiles[TilemapType.Vegetation]);
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
            var vegetationPositions = new Vector3Int[Chunk.ChunkSizeSquared];
            var vegetationTiles = new TileBase[Chunk.ChunkSizeSquared];
            
            var origin = chunk.Origin.ToVector3Int();
            var blocks = chunk.BlockStates;
            for (var i = 0; i < Chunk.ChunkSizeSquared; i++)
            {
                var block = blocks[i];

                if(BlockRegistry.Vegetation.Contains(block.Id))
                {
                    vegetationPositions[i] = origin + i.ToVector3Int0();
                    vegetationTiles[i] = block.Block.Tile;
                }
                else
                {
                    worldPositions[i] = origin + i.ToVector3Int0();
                    worldTiles[i] = block.Block.Tile;
                }

                backgroundPositions[i] = origin + i.ToVector3Int0();
                backgroundTiles[i] = block.WallId == 0 ? null : BlockRegistry.WallTiles[block.WallId] ;
                
            }

            return new Dictionary<TilemapType, (Vector3Int[], TileBase[])>
                   {
                       { TilemapType.World, (worldPositions, worldTiles) },
                       { TilemapType.Background, (backgroundPositions, backgroundTiles) },
                       { TilemapType.Vegetation, (vegetationPositions, vegetationTiles) }
                   };
        }

        public static void UnRenderChunk(Chunk chunk)
        {
            var origin = chunk.Origin.ToVector3Int();
            var positions = new Vector3Int[Chunk.ChunkSizeSquared];
            for (var i = 0; i < Chunk.ChunkSizeSquared; i++)
                positions[i] = origin + i.ToVector3Int0();
            var tiles = new TileBase[Chunk.ChunkSizeSquared];
            BackgroundTilemap.SetTiles(positions, tiles);
            VegetationTilemap.SetTiles(positions, tiles);
            WorldTilemap.SetTiles(positions, tiles);
        }
    }
}