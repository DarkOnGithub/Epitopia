using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Mono.CSharp;
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
        private static Dictionary<int, IBlock> _blockIds = new();
        private static Dictionary<string, IBlock> _blockNames = new();

        public static void RegisterBlocks()
        {
            BLOCK_AIR = RegisterBlock<DefaultBlock>("Air", new BlockProperties()
                                                           {
                                                               IsCollidable = false,
                                                               SpritePath = null
                                                           });
            BLOCK_NULL = RegisterBlock<DefaultBlock>("Null", new BlockProperties()
                                                             {
                                                                 SpritePath = "Null"
                                                             });
            BLOCK_DIRT = RegisterBlock<DefaultBlock>("Dirt", new BlockProperties()
                                                             {
                                                                 SpritePath = "dirt_low"
                                                             });
            BLOCK_GRASS = RegisterBlock<DefaultBlock>("Grass", new BlockProperties()
                                                               {
                                                                   SpritePath = "grass_low"
                                                               });
            BLOCK_STONE = RegisterBlock<DefaultBlock>("Stone", new BlockProperties()
                                                               {
                                                                   SpritePath = "stone_low"
                                                               });
            RegisterBlock<DefaultBlock>("Sand", new BlockProperties()
                                                               {
                                                                   SpritePath = "sand_low"
                                                               });
            RegisterBlock<DefaultBlock>("Water", new BlockProperties()
                                                               {
                                                                   SpritePath = "water_low"
                                                               });
        }
        private static string FormatName(string name)
        {
            return name.ToLower();
        }
        private static T RegisterBlock<T>(string blockName, BlockProperties properties) where T : IBlock
        {
            var instance = Activator.CreateInstance(typeof(T), new object[]
                                                               {
                                                                   _blockCount, blockName, properties
                                                               });
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