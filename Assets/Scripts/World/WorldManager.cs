using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using World.Chunks;

namespace World
{
    public class WorldManager : MonoBehaviour
    {
        public static WorldManager Instance;
        public static Dictionary<WorldIdentifier, AbstractWorld> Worlds = new();
        public static int RadiusHorizontal;
        public static int RadiusVertical;
        private const int RadiusPadding = 4;
        public static HashSet<Vector2Int> WaitingRoom = new(); 
        public static HashSet<Chunk> LoadedChunks = new();
        
        [SerializeField] public Tilemap tilemap;

        [SerializeField] public Grid Grid;
        public void Awake()
        {
            RegisterWorlds();
            Instance = this;
            Vector2 screenDimensions = new Vector2(
                Camera.main.orthographicSize * 2 * Camera.main.aspect,
                Camera.main.orthographicSize * 2
            );

            RadiusHorizontal = Mathf.CeilToInt(screenDimensions.x / Chunk.ChunkSize) + RadiusPadding;
            RadiusVertical = Mathf.CeilToInt(screenDimensions.y / Chunk.ChunkSize) + RadiusPadding;
            StartCoroutine(ChunkQueryloop());
        }
        public void RegisterWorlds()
        {
            RegisterWorld(new Overworld(WorldIdentifier.Overworld));
        } 
        IEnumerator ChunkQueryloop()
        {
            var wait = new WaitForSeconds(1/20f);
            while (1 == 0)
            {
                yield return wait;
                var localPlayer = Players.PlayerManager.LocalPlayer;
                
                if (localPlayer == null)continue;
                
                var worldIn = localPlayer.World;
                

                
                List<Vector2Int> chunksToRequest = new();
                var currentlyLoadedChunks = new HashSet<Chunk>(LoadedChunks);
                for(int x = -RadiusHorizontal; x < RadiusHorizontal; x++)
                {
                    for(int y = RadiusVertical; y < RadiusVertical; y++)
                    {
                        var chunkPosition = WorldQuery.FindNearestChunkPosition(new Vector2Int(x, y) + localPlayer.Position);
                        if (worldIn.GetChunk(chunkPosition, out var chunk))
                        {
                            LoadChunk(chunk);
                            currentlyLoadedChunks.Remove(chunk);
                        }
                        else
                            chunksToRequest.Add(chunkPosition);
                    }
                }
                worldIn.RequestChunks(chunksToRequest.ToArray(), new []{localPlayer.ClientId});
                foreach (var chunk in currentlyLoadedChunks)
                    UnLoadChunk(chunk);
                
            }
        }

        private static void LoadChunk(Chunk chunk)
        {
            if (chunk.IsDrawn) return;
            chunk.Draw();
            LoadedChunks.Add(chunk);
        }

        private static void UnLoadChunk(Chunk chunk)
        {
            
        }
        public static void RegisterWorld(AbstractWorld world)
        {
            Worlds.Add(world.Identifier, world);
        }
        
        public static AbstractWorld GetWorld(WorldIdentifier identifier)
        {
            if (Worlds.TryGetValue(identifier, out var world))
                return world;
            throw new Exception($"World {identifier} not found");
        }
    }
}