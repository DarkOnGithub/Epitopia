// using System;
// using System.Collections.Generic;
// using Tiles;
// using UnityEngine;
// using UnityEngine.Tilemaps;
//
// namespace World.Blocks
// {
//     public static class BlockRegistry
//     {
//         private static int _blockCount;
//         private static int _wallCount = 1;
//         public static DefaultBlock BLOCK_AIR;
//         public static DefaultBlock BLOCK_NULL;
//         public static DefaultBlock BLOCK_DIRT;
//         public static DefaultBlock BLOCK_GRASS;
//         public static DefaultBlock BLOCK_STONE;
//
//         public static Dictionary<int, TileBase> WallTiles = new();
//         private static readonly Dictionary<int, IBlock> _blockIds = new();
//         private static readonly Dictionary<string, IBlock> _blockNames = new();
//         public static HashSet<int> Walls = new();
//         public static HashSet<int> Vegetation = new();
//         public static void RegisterBlocks()
//         {
//             BLOCK_AIR = RegisterBlock<DefaultBlock>("Air", new BlockProperties
//                                                            {
//                                                                SpritePath = null,
//                                                                IsSolid = false
//                                                            });
//             // BLOCK_NULL = RegisterBlock<DefaultBlock>("Null", new BlockProperties()
//             //                                                  {
//             //                                                      SpritePath = "Null"
//             //                                                  });
//             BLOCK_DIRT = RegisterBlock<DefaultBlock>("Dirt", new BlockProperties
//                                                              {
//                                                                  SpritePath = "Dirt",
//                                                              });
//             BLOCK_GRASS = RegisterBlock<DefaultBlock>("Grass", new BlockProperties
//                                                                {
//                                                                    SpritePath = "Grass",
//                                                                });
//             BLOCK_STONE = RegisterBlock<DefaultBlock>("Stone", new BlockProperties
//                                                                {
//                                                                    SpritePath = "Stone",
//                                                                });
//
//             RegisterTile("Dirt");
//             RegisterTile("Stone");
//             RegisterBlock<DefaultBlock>("Sand", new BlockProperties()
//                                                                {
//                                                                    SpritePath = "Sand"
//                                                                });
//                 RegisterBlock<DefaultBlock>("DryGrass", new BlockProperties()
//                                                 {
//                                                     SpritePath = "DryGrass"
//                                                 });
//                 RegisterBlock<FlowerBlock>("Flower", new BlockProperties()
//                                                 {
//                                                     SpritePath = "Flowers",
//                                                     IsSolid = false
//                                                 });
//                 RegisterBlock<TreeBlock>("TreeBlock", new BlockProperties(){ });
//
//         }
//
//         private static string FormatName(string name)
//         {
//             return name.ToLower();
//         }
//
//         private static T RegisterBlock<T>(string blockName, BlockProperties properties) where T : IBlock
//         {
//             var instance = Activator.CreateInstance(typeof(T), _blockCount, blockName, properties);
//
//             _blockIds[_blockCount++] = (IBlock)instance;
//             _blockNames[FormatName(blockName)] = (IBlock)instance;
//             return (T)instance;
//         }
//
//         private static void RegisterTile(string name)
//         {
//             var tile = new WallTile(Resources.Load<Texture2D>($"Sprites/Blocks/{name}_Wall"));
//             WallTiles[_wallCount++] = tile.RuleTile;
//         }
//
//         public static IBlock GetBlock(int id)
//         {
//             return _blockIds.GetValueOrDefault(id, BLOCK_NULL);
//         }
//
//         public static IBlock GetBlock(string name)
//         {
//             return _blockNames.GetValueOrDefault(FormatName(name), BLOCK_NULL);
//         }
//     }
// }

using System.Collections.Generic;
using World.Blocks.CustomBlocks;

namespace World.Blocks
{
    public static class BlockRegistry
    {
        public static DefaultBlock BlockAir;
        public static DefaultBlock BlockNull;
        
        
        private static readonly List<AbstractBlock> IdsRegistry = new();
        private static readonly Dictionary<string, AbstractBlock> NamesRegistry = new();
        static BlockRegistry()
        {
            RegisterBlocks();    
        }
        
        public static void RegisterBlocks()
        {
            BlockAir = RegisterBlock<DefaultBlock>("Air", new BlockProperties
            {
                SpritePath = null,
                IsSolid = false
            });
            
            BlockNull = RegisterBlock<DefaultBlock>("Null", new BlockProperties
            {
                SpritePath = "Null"
            });
        }
        
        public static T RegisterBlock<T>(string blockName, BlockProperties properties) where T : AbstractBlock
        {
            var instance = System.Activator.CreateInstance(typeof(T), IdsRegistry.Count, blockName, properties);

            IdsRegistry.Add((AbstractBlock)instance);
            NamesRegistry[blockName.ToLower()] = (AbstractBlock)instance;
            return (T)instance;
        }
        
        public static AbstractBlock GetBlock(int id)
        {
            if (id < 0 || id >= IdsRegistry.Count)
                return null;
            return IdsRegistry[id];
        }
        
        public static AbstractBlock GetBlock(string name)
        {
            return NamesRegistry[name.ToLower()];
        }
    }
}