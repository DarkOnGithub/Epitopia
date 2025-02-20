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

using System.Collections.Concurrent;
using System.Collections.Generic;
using Tiles;
using UnityEngine;
using UnityEngine.Tilemaps;
using World.Blocks.CustomBlocks;

namespace World.Blocks
{
    public static class BlockRegistry
    {
        public static DefaultBlock BlockAir;
        public static DefaultBlock BlockNull;

        public static ConcurrentDictionary<int, byte> VegetationBlocks = new();
        public static ConcurrentDictionary<int, byte> WallBlocks = new();
        

        public static readonly ConcurrentDictionary<int, TileBase> WallTiles = new();
        public static readonly ConcurrentDictionary<int, AbstractBlock> IdsRegistry = new();
        public static readonly ConcurrentDictionary<string, AbstractBlock> NamesRegistry = new();
        static BlockRegistry()
        {
            if (IdsRegistry.Count == 0) 
                RegisterBlocks();    
        }
        
        public static void RegisterBlocks()
        {
            if (IdsRegistry.Count != 0)
                return;
            RegisterWall("Dirt");
            RegisterWall("Stone");
            BlockAir = RegisterBlock<DefaultBlock>("Air", new BlockProperties
            {
                SpritePath = null,
                IsSolid = false,
            });
            
            BlockNull = RegisterBlock<DefaultBlock>("Null", new BlockProperties
            {
                SpritePath = null,

            });
            RegisterWithVariations<GrassBlock>("Grass", new[]{"Dry"}, new BlockProperties()
            {
                SpritePath = "Grass",
                MergeWithDirt = true
            });
          
            RegisterBlock<DirtBlock>("Dirt", new BlockProperties()
            {
                SpritePath = "Dirt",
                MergeWithDirt = true

            });
            
            RegisterBlock<DefaultBlock>("Stone", new BlockProperties()
            {
                SpritePath = "Stone",
                MergeWithDirt = true
            });
                                
            RegisterBlock<DefaultBlock>("Sand", new BlockProperties()
            {
                SpritePath = "Sand",
                MergeWithDirt = true

            });       
            RegisterWithVariations<FlowerBlock>("Flower", new []{"Dry"}, new BlockProperties()
                                                                         {
                                                                             SpritePath = "Flower"
                                                                         });
            
           RegisterBlock<TreeBlock>("TreeLog", new BlockProperties()
           {
               SpritePath = "Tree_Log"
           });
            RegisterWithVariations<TreeLeaves>("", new[]{"Oak_Leaves","Savanna_Leaves"}, new BlockProperties()
            {
                SpritePath = "Tree"
            });           
            RegisterWithVariations<TreeTop>("", new[]{"Oak_Top","Savanna_Top"}, new BlockProperties()
            {
                SpritePath = "Tree"
            });
        }

        private static void RegisterWithVariations<T>(string baseName, string[] variations, BlockProperties properties) where T : AbstractBlock
        {
            if(baseName != "")
                RegisterBlock<T>(baseName, properties);
            foreach (var variation in variations)
            {
                var cpy = (BlockProperties)properties.Clone();
                cpy.SpritePath = $"{cpy.SpritePath}_{variation}";
                Debug.Log(cpy.SpritePath);
                RegisterBlock<T>(baseName +" " + variation, cpy);
                
            }
        }
        
        public static void RegisterWall(string wallName)
        {
            WallTiles.TryAdd(WallTiles.Count + 1, new WallTile(wallName).RuleTile);
            DefaultRuleTile.Walls.Add(WallTiles[WallTiles.Count]);
        }
        
        public static T RegisterBlock<T>(string blockName, BlockProperties properties) where T : AbstractBlock
        {

            var instance = System.Activator.CreateInstance(typeof(T), IdsRegistry.Count, blockName, properties);

            IdsRegistry.TryAdd(IdsRegistry.Count, (AbstractBlock)instance);
            NamesRegistry[CleanString(blockName)] = (AbstractBlock)instance;
            return (T)instance;
        }
        
        public static AbstractBlock GetBlock(int id)
        {
            if (id < 0 || id >= IdsRegistry.Count)
                return null;
            return IdsRegistry[id];
        }

        private static string CleanString(string name)
        {
            return name.ToLower().Replace("_","").Replace(" ","");
        }
        public static AbstractBlock GetBlock(string name)
        {
            return NamesRegistry[CleanString(name)];
        }
    }
}