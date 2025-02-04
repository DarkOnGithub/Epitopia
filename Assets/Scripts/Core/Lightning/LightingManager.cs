// using System;
// using System.Collections.Generic;
// using System.Diagnostics;
// using Events.EventHandler.Holders;
// using QFSW.QC.Editor.Tools;
// using UnityEngine;
// using UnityEngine.UIElements;
// using Utils;
// using World;
// using World.Blocks;
// using World.Chunks;
// using Debug = UnityEngine.Debug;
//
// namespace Core.Lightning
// {
//     public class LightingManager
//     {
//         private GameObject _lightmapObject;
//         private static readonly Vector2Int[] Directions = {
//                                                               new(1, 0), new(-1, 0), new(0, 1), new(0, -1),
//                                                               new(1, 1), new(1, -1), new(-1, 1), new(-1, -1)
//                                                           };
//         private static readonly Queue<(Vector2Int Position, int Level)> Queue = new(256);
//         private static readonly HashSet<Vector2Int> Visited = new(256);
//         public AbstractWorld World;
//         
//         public LightingManager(AbstractWorld worldIn)
//         {
//             World = worldIn;
//             _lightmapObject = Resources.Load<GameObject>("Prefabs/Lightmap");
//             _lightmapObject.transform.localScale = new Vector3(Chunk.ChunkSize, Chunk.ChunkSize, 1);
//         
//         }
//
//        // public void ComputeSkylight(Chunk chunk)
//        // {
//        //     // Get the chunk above the current chunk
//        //     World.Query.FindNearestChunk(chunk.Center + Vector2Int.up * Chunk.ChunkSize, out var aboveChunk);
//        //     
//        //     // Cache surface levels for the current chunk
//        //     var surfaceLevels = chunk.World.WorldGenerator.HeightMap.CacheHeight(chunk.Origin);
//        //     
//        //     // Iterate through each column in the chunk
//        //     for (int x = 0; x < Chunk.ChunkSize; x++)
//        //     {
//        //         // Get initial light level from chunk above or default to full brightness (16)
//        //         var aboveLight = aboveChunk?.GetBlock(x)?.LightLevel ?? 16;
//        //         var surfaceLevel = surfaceLevels.GetPoint(x);
//        //         
//        //         // Process each block in the column from top to bottom
//        //         for (int y = Chunk.ChunkSize - 1; y >= 0; y--)
//        //         {
//        //             var block = chunk.GetBlock(x + y * Chunk.ChunkSize);
//        //             
//        //             // Calculate absolute Y position in the world
//        //             int worldY = y + chunk.Origin.y;
//        //             
//        //             if (worldY > surfaceLevel)
//        //             {
//        //                 // Above surface: full light if not solid, otherwise reduce by distance from surface
//        //                 if (!block.Properties.IsSolid)
//        //                 {
//        //                     block.LightLevel = 16;
//        //                 }
//        //                 else
//        //                 {
//        //                     block.LightLevel = (byte)Mathf.Max(0, aboveLight - (worldY - surfaceLevel));
//        //                 }
//        //             }
//        //             else
//        //             {
//        //                 // Below surface: reduce light by 1 for each block down, but don't go below 0
//        //                 block.LightLevel = (byte)Mathf.Max(0, aboveLight - 1);
//        //             }
//        //             
//        //             // Update aboveLight for next iteration
//        //             aboveLight = block.LightLevel;
//        //         }
//        //     }
//        // }
//
//         public static void AddLightmap(Chunk chunk)
//         {
//             
//             var lightmap = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Lightmap"));
//             chunk.LightmapObject = lightmap;
//             lightmap.transform.position = chunk.Center.ToVector3Int();
//             var lightTexture = new Texture2D(Chunk.ChunkSize, Chunk.ChunkSize);
//             for (int x = 0; x < Chunk.ChunkSize; x++)
//             {
//                 for (int y = 0; y < Chunk.ChunkSize; y++)
//                 {
//                     var color = new Color(0, 0, 0, 1);
//                     lightTexture.SetPixel(x, y, color);
//                 }
//             }
//             lightTexture.filterMode = FilterMode.Point;
//             lightTexture.Apply();
//             chunk.LightTexture = lightTexture;
//             lightmap.GetComponent<SpriteRenderer>().material.SetTexture("_Lightmap", lightTexture);
//         }
//
//         public static void UpdateChunkLightmap(Chunk chunk)
//         {
//             var chunkSize = Chunk.ChunkSize;
//             var lightTexture = chunk.LightTexture;
//             for(int x = 0; x < chunkSize; x++)
//             {
//                 for(int y = 0; y < chunkSize; y++)
//                 {
//                     var block = chunk.GetBlock(x + y * chunkSize);
//                     var color = new Color(0, 0, 0, 1 - Mathf.Min(15, block.LightLevel) / 15f);
//                     lightTexture.SetPixel(x, y, color);
//                 }
//             }
//             lightTexture.Apply();
//         }
//
//       
//         
//
//         public void PropagateLight(Vector2Int start, byte lightStrength, Chunk sourceChunk, HashSet<Chunk> updatedChunks)
//         {
//             var query = World.Query;
//             var startPosition = start + sourceChunk.Origin;
//             Chunk chunk;
//             if (!query.FindNearestChunk(startPosition, out chunk))
//                 return;
//     
//             updatedChunks.Add(chunk);
//             Queue.Clear();
//             Visited.Clear();
//
//             var startBlock = chunk.GetBlock(WorldUtils.WorldToLocalPosition(startPosition, chunk.Origin).ToIndex());
//             if (startBlock == null || startBlock.LightLevel >= lightStrength) return; 
//     
//             startBlock.LightLevel = lightStrength;
//             Queue.Enqueue((startPosition, lightStrength));
//             Visited.Add(startPosition);
//
//             while (Queue.Count > 0)
//             {
//                 var (currentPos, currentLevel) = Queue.Dequeue();
//                 var nextLevel = (byte)(currentLevel - 1);
//
//                 if (nextLevel <= 0) continue;
//
//                 for (int i = 0; i < Directions.Length; i++)
//                 {
//                     var nextPos = currentPos + Directions[i];
//             
//                     if (!Visited.Add(nextPos)) continue;
//
//                     if (!query.FindNearestChunk(nextPos, out chunk)) continue; 
//
//                     var nextBlock = chunk.GetBlock(WorldUtils.WorldToLocalPosition(nextPos, chunk.Origin).ToIndex());
//                     if (nextBlock == null || nextBlock.LightLevel >= nextLevel) continue; 
//             
//                     updatedChunks.Add(chunk);
//                     nextBlock.LightLevel = nextLevel;
//                     Queue.Enqueue((nextPos, nextLevel));
//                 }
//             }
//         }
//     }
// }