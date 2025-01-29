using UnityEngine;
using World.Chunks;
using World.WorldGeneration.Noise;

namespace World.WorldGeneration.Structures
{
    public class StructurePlacement
    {
        //Unit is chunk
        public const int SuperChunkRadius = 16;
        public const int SuperChunkSize = SuperChunkRadius * Chunk.ChunkSize;
        private NoiseGenerator _noiseGenerator = new("BgA=", 0.02f);
        public void LoadSuperchunk(Vector2Int start)
        {
            _noiseGenerator.GenerateCache(start, new Vector2Int(SuperChunkSize, 1));

            for (var x = 0; x < SuperChunkRadius; x++)
            {
                
            }
        }
    }
}