using System;
using JetBrains.Annotations;

namespace World.Blocks
{
    public static class BlockRegistry
    {
        private static int _blockCount;
        public static DefaultBlock BLOCK_AIR;
        public static DefaultBlock BLOCK_NULL;
        public static DefaultBlock BLOCK_DIRT;

        static void RegisterBlocks()
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

        private static T RegisterBlock<T>(string blockName, BlockProperties properties, [CanBeNull] params object[] args) where T : IBlock
        {
            return (T)Activator.CreateInstance(typeof(T), new object[]{
                          _blockCount++, blockName, properties, args
                      });
        }
    }
}