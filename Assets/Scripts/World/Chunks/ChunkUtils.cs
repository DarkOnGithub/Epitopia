using MessagePack;
using Utils.LZ4;
using World.Blocks;

namespace World.Chunks
{
    public static class ChunkUtils
    {
        public static byte[] CompressChunkContent(byte[] content)
        {
            return LZ4.Compress(content);
        }

        public static byte[] SerializeChunk(ChunkData data)
        {
           return MessagePackSerializer.Serialize(data);
        }
        
        public static byte[] SerializeAndCompressChunk(Chunk chunk)
        {
            return CompressChunkContent(SerializeChunk(chunk.GetChunkData()));
        }
        
        public static ChunkData DeserializeChunk(byte[] data)
        {
            return MessagePackSerializer.Deserialize<ChunkData>(data);
        }

        public static byte[] DecompressChunkContent(byte[] content)
        {
            return LZ4.Decompress(content);
        }
        
        public static ChunkData DeserializeAndDecompressChunk(byte[] data)
        {
            return DeserializeChunk(DecompressChunkContent(data));
        }
    }
}