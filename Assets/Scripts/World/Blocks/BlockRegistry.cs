using System;
using System.Collections.Generic;
using UnityEngine;

namespace World.Blocks
{
    public static class BlockRegistry
    {
        private static int _blockCount;
        public static DefaultBlock BLOCK_AIR;
        public static DefaultBlock BLOCK_NULL;
        public static DefaultBlock BLOCK_DIRT;
        public static DefaultBlock BLOCK_GRASS;
        public static DefaultBlock BLOCK_STONE;
        public static BaseWall WALL_DIRT;
        public static BaseWall WALL_STONE;

        private static readonly Dictionary<int, IBlock> _blockIds = new();
        private static readonly Dictionary<string, IBlock> _blockNames = new();
        public static HashSet<int> WallIds = new();

        public static void RegisterBlocks()
        {
            BLOCK_AIR = RegisterBlock<DefaultBlock>("Air", new BlockProperties
                                                           {
                                                               SpritePath = null,
                                                               IsSolid = false
                                                           });
            // BLOCK_NULL = RegisterBlock<DefaultBlock>("Null", new BlockProperties()
            //                                                  {
            //                                                      SpritePath = "Null"
            //                                                  });
            BLOCK_DIRT = RegisterBlock<DefaultBlock>("Dirt", new BlockProperties
                                                             {
                                                                 SpritePath = "Dirt",
                                                             });
            BLOCK_GRASS = RegisterBlock<DefaultBlock>("Grass", new BlockProperties
                                                               {
                                                                   SpritePath = "Grass",
                                                               });
            BLOCK_STONE = RegisterBlock<DefaultBlock>("Stone", new BlockProperties
                                                               {
                                                                   SpritePath = "Stone",
                                                               });
            WALL_DIRT = RegisterBlock<BaseWall>("Dirt_Wall", new BlockProperties
                                                             {
                                                                 SpritePath = "Dirt_Wall"
                                                             });
            WALL_STONE = RegisterBlock<BaseWall>("Stone_Wall", new BlockProperties
                                                               {
                                                                   SpritePath = "Stone_Wall"
                                                               });
            RegisterBlock<DefaultBlock>("Sand", new BlockProperties()
                                                               {
                                                                   SpritePath = "Sand"
                                                               });
                RegisterBlock<DefaultBlock>("DryGrass", new BlockProperties()
                                                {
                                                    SpritePath = "DryGrass"
                                                });
                RegisterBlock<FlowerBlock>("Flower", new BlockProperties()
                                                {
                                                    SpritePath = "Flowers",
                                                    IsSolid = false
                                                });
            // RegisterBlock<DefaultBlock>("Water", new BlockProperties()
            //                                                    {
            //                                                        SpritePath = "water_low"
            //                                                    });
        }

        private static string FormatName(string name)
        {
            return name.ToLower();
        }

        private static T RegisterBlock<T>(string blockName, BlockProperties properties) where T : IBlock
        {
            var instance = Activator.CreateInstance(typeof(T), _blockCount, blockName, properties);
            if (instance is BaseWall wall)
                WallIds.Add(wall.Id);
            _blockIds[_blockCount++] = (IBlock)instance;
            _blockNames[FormatName(blockName)] = (IBlock)instance;
            return (T)instance;
        }

        public static IBlock GetBlock(int id)
        {
            return _blockIds.GetValueOrDefault(id, BLOCK_NULL);
        }

        public static IBlock GetBlock(string name)
        {
            return _blockNames.GetValueOrDefault(FormatName(name), BLOCK_NULL);
        }
    }
}