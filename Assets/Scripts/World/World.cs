using System.Collections.Generic;
using Blocks;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.XR;
using Utils;
using World.Chunks;

namespace World
{
    public abstract class World
    {
        public Dictionary<Vector2Int, Chunk> Chunks = new();
        public Tilemap Tilemap;
        public Grid Grid;
        public World()
        {
            GameObject.Find("Tilemap").TryGetComponent(out Tilemap);
            GameObject.Find("Grid").TryGetComponent(out Grid);
        }
        public void GenerateChunk(Vector2Int position)
        {
            if (Chunks.ContainsKey(position))
                return;
            Chunks[position] = new Chunk(position);
        }
        public bool SetBlock(Vector2Int worldPosition, IBlockState block)
        {
            if (WorldQuery.SetBlock(this, worldPosition, block))
            {
                Tilemap.SetTile(
                    Grid.WorldToCell(worldPosition.ToVector3Int()), 
                           block.Block.Tile
                    );
                return true;       
            }

            return false;
        }
    }
}