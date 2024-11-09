using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace World.Blocks
{
    public static class BlockRegistry
    {
        private static int _blockCount;
        public static DefaultBlock BLOCK_AIR;
        public static DefaultBlock BLOCK_NULL;
        public static DefaultBlock BLOCK_DIRT;
        private static Dictionary<int, IBlock> _blocks = new();
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
                                                                 SpritePath = "Dirt"
                                                             });
        }

        private static T RegisterBlock<T>(string blockName, BlockProperties properties) where T : IBlock
        {
            var instance = Activator.CreateInstance(typeof(T), new object[]{
                          _blockCount++, blockName, properties
                      });
            _blocks[_blockCount++] = (IBlock)instance;
            return (T)instance;
        }
        
        public static IBlock GetBlock(int id)
        {
            return id switch
                   {
                       0 => BLOCK_AIR,
                       1 => BLOCK_NULL,
                       2 => BLOCK_DIRT,
                       _ => BLOCK_NULL
                   };
        }
    }
}