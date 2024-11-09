using MessagePack;
using UnityEngine;
using Utils.LZ4;
using World.Blocks;

namespace World.Chunks
{
    public static class ChunkUtils
    {
        private static readonly MessagePackSerializerOptions Options = MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4BlockArray);

        public static byte[] SerializeChunk(Chunk chunk)
        {
            var data = chunk.GetChunkData();
           return MessagePackSerializer.Serialize(data, Options);
        }
        
        public static ChunkData DeserializeChunk(byte[] data)
        {
            return MessagePackSerializer.Deserialize<ChunkData>(data, Options);
        }

    }
}