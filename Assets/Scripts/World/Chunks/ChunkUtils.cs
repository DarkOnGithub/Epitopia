using MessagePack;
using UnityEngine;
using Utils.LZ4;
using World.Blocks;

namespace World.Chunks
{
    public static class ChunkUtils
    {
        public static readonly MessagePackSerializerOptions Options = MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4BlockArray);

        public static byte[] SerializeChunk(Chunk chunk, bool compress = true)
        {
            var data = chunk.GetChunkData();
           return MessagePackSerializer.Serialize(data, compress ? Options : null);
        }
        
        public static ChunkData DeserializeChunk(byte[] data, bool compress = true)
        {
            return MessagePackSerializer.Deserialize<ChunkData>(data, compress ? Options : null);
        }

    }
}