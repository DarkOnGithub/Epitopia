using System.Linq;
using MessagePack;
using Network.Messages;
using Network.Messages.Packets.World;
using PimDeWitte.UnityMainThreadDispatcher;
using Unity.Netcode;
using UnityEngine;
using Utils.LZ4;
using World.Blocks;

namespace World.Chunks
{
    public static class ChunkUtils
    {
        public static readonly MessagePackSerializerOptions Options =
            MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4BlockArray);

        public static byte[] SerializeChunk(Chunk chunk, bool compress = true)
        {
            var data = chunk.GetChunkData();
            return MessagePackSerializer.Serialize(data, compress ? Options : null);
        }

        public static ChunkData DeserializeChunk(byte[] data, bool compress = true)
        {
            return MessagePackSerializer.Deserialize<ChunkData>(data, compress ? Options : null);
        }

        public static void SendChunk(Chunk chunk)
        {
            var serializedChunk = SerializeChunk(chunk, true);

            MessageFactory.SendPacket(SendingMode.ServerToClient, new ChunkTransferMessage
            {
                ChunkData = serializedChunk,
                Position = chunk.Center,
                World = chunk.World.Identifier
            }, chunk.Players.ToArray(), null, NetworkDelivery.ReliableFragmentedSequenced);
        }

        public static void SendChunkFromThread(Chunk chunk)
        {
            var serializedChunk = SerializeChunk(chunk, true);
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                MessageFactory.SendPacket(SendingMode.ServerToClient, new ChunkTransferMessage
                {
                    ChunkData = serializedChunk,
                    Position = chunk.Center,
                    World = chunk.World.Identifier
                }, chunk.Players.ToArray(), null, NetworkDelivery.ReliableFragmentedSequenced);
            });
        }
    }
}