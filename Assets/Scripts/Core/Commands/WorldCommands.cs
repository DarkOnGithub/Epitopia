using System.Linq;
using Players;
using QFSW.QC;
using UnityEngine;
using World.Blocks;
using World.Chunks;
using World.WorldGeneration;


namespace Core.Commands
{
    public class WorldCommands
    {
        [Command]
        public static void RegenWorld()
        {
            var player = PlayerManager.LocalPlayer;
            var chunks = player.World.Chunks.Values.ToArray();
            for(int i=0; i< chunks.Length; i++)
            {
                var chunk = chunks[i];
                var random = new System.Random().Next(-2,2);
                for (int x = 0; x < Chunk.ChunkSize; x++)
                {
                    for (int y = 0; y < Chunk.ChunkSize; y++)
                    {
                        float noise = Mathf.PerlinNoise((x + chunk.Origin.x + random) * 0.1f, (y + chunk.Origin.y + random) * 0.1f);
                        int height = Mathf.FloorToInt(noise * Chunk.ChunkSize);

                        if (y + chunk.Origin.y < height)
                        {
                            chunk.SetBlock(x + y * Chunk.ChunkSize, BlockRegistry.BLOCK_DIRT.GetDefaultState());
                        }
                    }
                }
                Debug.Log($"Regen: {chunk.Center}");

                player.World.SendChunkToServer(chunk);
            }

            //         Overworld.Instance.SendChunkToServer(Overworld.Instance.Chunks.First().Value);
        }
    }
}