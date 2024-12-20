using System;
using Network.Messages;
using Network.Messages.Packets.World;
using UnityEngine;
using World.Chunks;

namespace World
{
    public class ClientWorldHandler : IDisposable
    {
        private bool _disposed = false;
        public AbstractWorld WorldIn { get; }

        public ClientWorldHandler(AbstractWorld worldIn)
        {
            WorldIn = worldIn;
        }

        public void OnPacketReceived(NetworkUtils.Header header, ChunkTransferMessage message)
        {
            var chunkData = ChunkUtils.DeserializeChunk(message.ChunkData);
            chunkData.Center = message.Position;
            var chunk = WorldIn.Query.GetChunkOrCreate(chunkData.Center);
            var content = chunkData.BlockStates;
            chunk.UpdateContent(content);
        }

        public void DropChunks(Vector2Int[] positions)
        {
            MessageFactory.SendPacket(SendingMode.ClientToServer, new ChunkRequestMessage
            {
                Positions = positions,
                Type = ChunkRequestType.Drop,
                World = WorldIn.Identifier
            });
        }

        public void RemoveChunk(Chunk chunk)
        {
            WorldIn.Query.RemoveChunk(chunk.Center);
            DropChunks(new[] { chunk.Center });
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
                foreach (var chunk in WorldIn.Query.Chunks.Values)
                    chunk.Dispose();
            _disposed = true;
        }

        ~ClientWorldHandler()
        {
            Dispose(false);
        }
    }
}