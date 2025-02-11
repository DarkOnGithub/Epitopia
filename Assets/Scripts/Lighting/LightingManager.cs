// using System.Collections.Generic;
// using System.Linq;
// using UnityEngine;
// using Utils;
// using World;
// using World.Blocks;
// using World.Chunks;
//
// namespace Lighting
// {
//     public class LightingManager
//     {
//         private AbstractWorld _worldIn;
//
//         public LightingManager(AbstractWorld worldIn)
//         {
//             _worldIn = worldIn;
//         }
//
//         /// <summary>
//         /// Recalculates the lighting for a given chunk (and uses its immediate neighbors).
//         /// Both sky light and block light (from sources like torches) are propagated.
//         /// </summary>
//         public void UpdateLighting(Chunk chunk, Chunk topChunk, Chunk bottomChunk, Chunk rightChunk, Chunk leftChunk)
//         {
//             // Reset lighting for the current chunk.
//             foreach (var block in chunk.BlockStatesRef)
//             {
//                 block.LightLevel = 0;
//             }
//
//             PropagateSkyLight(chunk, topChunk, bottomChunk, rightChunk, leftChunk);
//             PropagateBlockLight(chunk, topChunk, bottomChunk, rightChunk, leftChunk);
//         }
//
//         /// <summary>
//         /// Propagates sky light using BFS.
//         /// For each block above the column’s maximum height, full brightness (15) is applied.
//         /// Then, for each neighboring block the candidate light level is computed as:
//         ///     candidate = neighbor's light - (baseDecay + extraDecay)
//         /// where baseDecay is always 1 per block and extraDecay is 1 if the block is solid.
//         /// A block’s light level is updated only if this candidate is higher than its current light.
//         /// </summary>
//         private void PropagateSkyLight(Chunk chunk, Chunk top, Chunk bottom, Chunk right, Chunk left)
//         {
//             var serverHandler = chunk.WorldIn.ServerHandler;
//             int chunkWorldY = chunk.Position.y;
//             Queue<LightNode> queue = new Queue<LightNode>();
//
//             // Initialize sky-lit blocks (full brightness above the maximum height in each column).
//             for (int x = 0; x < Chunk.ChunkSize; x++)
//             {
//                 if(!serverHandler.HeightsPerColumn.TryGetValue(chunk.Position.x + x, out var columnHeights))
//                    continue;
//                 int maxY = columnHeights.Keys.Max();
//                 for (int y = 0; y < Chunk.ChunkSize; y++)
//                 {
//                     int worldY = chunkWorldY + y;
//                     if (worldY > maxY)
//                     {
//                         var block = chunk.BlockStatesRef[(x, y).ToIndex()];
//                         block.LightLevel = 15;
//                         queue.Enqueue(new LightNode { Chunk = chunk, X = x, Y = y, LightLevel = 15 });
//                     }
//                 }
//             }
//
//             // BFS propagation: For each neighbor, compute candidate light level and update if higher.
//             while (queue.Count > 0)
//             {
//                 LightNode node = queue.Dequeue();
//
//                 foreach (var offset in new (int dx, int dy)[] { (-1, 0), (1, 0), (0, -1), (0, 1) })
//                 {
//                     int nx = node.X + offset.dx;
//                     int ny = node.Y + offset.dy;
//
//                     var (neighborChunk, localX, localY) = GetNeighborChunk(node.Chunk, nx, ny, top, bottom, left, right);
//                     if (neighborChunk == null)
//                         continue;
//
//                     var neighborBlock = neighborChunk.BlockStatesRef[(localX, localY).ToIndex()];
//
//                     // Calculate attenuation:
//                     // Always lose 1 light per block (baseDecay), plus an extra 1 if the neighbor is solid.
//                     int baseDecay = 1;
//                     int extraDecay = BlockRegistry.GetBlock(neighborBlock.Id).Properties.IsTransparent ? 0 : 2;
//                     int candidateLight = node.LightLevel - baseDecay - extraDecay;
//
//                     // Update only if the candidate light is higher than the neighbor’s current light level.
//                     if (candidateLight > neighborBlock.LightLevel)
//                     {
//                         neighborBlock.LightLevel = candidateLight;
//                         if (candidateLight > 0)
//                         {
//                             queue.Enqueue(new LightNode { Chunk = neighborChunk, X = localX, Y = localY, LightLevel = candidateLight });
//                         }
//                     }
//                 }
//             }
//         }
//
//         /// <summary>
//         /// Propagates light emitted by blocks (such as torches) using BFS.
//         /// For each light source, the candidate light level for a neighbor is computed similarly:
//         ///     candidate = current light - (baseDecay + extraDecay)
//         /// and the neighbor is updated only if the candidate is higher.
//         /// </summary>
//         private void PropagateBlockLight(Chunk chunk, Chunk top, Chunk bottom, Chunk right, Chunk left)
//         {
//             Queue<LightNode> queue = new Queue<LightNode>();
//
//             // Enqueue all blocks that emit light.
//             for (int x = 0; x < Chunk.ChunkSize; x++)
//             {
//                 for (int y = 0; y < Chunk.ChunkSize; y++)
//                 {
//                     var block = chunk.BlockStatesRef[(x, y).ToIndex()];
//                     int emission = BlockRegistry.GetBlock(block.Id).Properties.LightEmission;
//                     if (emission > 0)
//                     {
//                         if (emission > block.LightLevel)
//                         {
//                             block.LightLevel = emission;
//                             queue.Enqueue(new LightNode { Chunk = chunk, X = x, Y = y, LightLevel = emission });
//                         }
//                     }
//                 }
//             }
//
//             // BFS propagation for block light.
//             while (queue.Count > 0)
//             {
//                 LightNode node = queue.Dequeue();
//
//                 foreach (var offset in new (int dx, int dy)[] { (-1, 0), (1, 0), (0, -1), (0, 1) })
//                 {
//                     int nx = node.X + offset.dx;
//                     int ny = node.Y + offset.dy;
//
//                     var (neighborChunk, localX, localY) = GetNeighborChunk(node.Chunk, nx, ny, top, bottom, left, right);
//                     if (neighborChunk == null)
//                         continue;
//
//                     var neighborBlock = neighborChunk.BlockStatesRef[(localX, localY).ToIndex()];
//
//                     // Calculate attenuation:
//                     int baseDecay = 1;
//                     int extraDecay = BlockRegistry.GetBlock(neighborBlock.Id).Properties.IsTransparent ? 0 : 1;
//                     int candidateLight = node.LightLevel - baseDecay - extraDecay;
//
//                     if (candidateLight > neighborBlock.LightLevel)
//                     {
//                         neighborBlock.LightLevel = candidateLight;
//                         if (candidateLight > 0)
//                         {
//                             queue.Enqueue(new LightNode { Chunk = neighborChunk, X = localX, Y = localY, LightLevel = candidateLight });
//                         }
//                     }
//                 }
//             }
//         }
//
//         /// <summary>
//         /// Helper method that returns the proper neighbor chunk and local coordinates if (x,y)
//         /// lies outside the current chunk.
//         /// </summary>
//         /// <returns>A tuple of (Chunk, x, y) or (null, x, y) if no neighbor exists.</returns>
//         private (Chunk chunk, int x, int y) GetNeighborChunk(Chunk current, int x, int y, Chunk top, Chunk bottom, Chunk left, Chunk right)
//         {
//             if (x < 0)
//             {
//                 if (left == null)
//                     return (null, x, y);
//                 return (left, Chunk.ChunkSize - 1, y);
//             }
//             if (x >= Chunk.ChunkSize)
//             {
//                 if (right == null)
//                     return (null, x, y);
//                 return (right, 0, y);
//             }
//             if (y < 0)
//             {
//                 if (bottom == null)
//                     return (null, x, y);
//                 return (bottom, x, Chunk.ChunkSize - 1);
//             }
//             if (y >= Chunk.ChunkSize)
//             {
//                 if (top == null)
//                     return (null, x, y);
//                 return (top, x, 0);
//             }
//             return (current, x, y);
//         }
//
//         /// <summary>
//         /// Example method that “destroys” a block by replacing it with an air block (which should be transparent)
//         /// and then updating the lighting for the affected chunk.
//         /// </summary>
//         public void DestroyBlock(Chunk chunk, int x, int y, Chunk top, Chunk bottom, Chunk right, Chunk left)
//         {
//             // Replace the block with an “air” block here (assume Block.Air is a valid transparent, non–emitting block)
//             // Then update the lighting.
//             UpdateLighting(chunk, top, bottom, right, left);
//         }
//
//         // A helper struct used for BFS propagation.
//         private struct LightNode
//         {
//             public Chunk Chunk;
//             public int X;
//             public int Y;
//             public int LightLevel;
//         }
//     }
// }
