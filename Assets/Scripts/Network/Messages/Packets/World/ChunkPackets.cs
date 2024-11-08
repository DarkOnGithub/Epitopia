using UnityEngine;
using Utils.LZ4;
using World;

namespace Network.Messages.Packets.World
{
    public enum RequestState
    {
        Drop = 0,
        Request = 1,
    }
    public struct ChunkRequestData : IMessageData
    {
        public Vector2Int[] ChunksPosition;
        public RequestState State;
    }
    public class ChunkRequestHandler : NetworkPacket<ChunkRequestData>
    {
        public override NetworkMessageIdenfitier Identifier { get; } = NetworkMessageIdenfitier.World;
        protected override void OnPacketReceived(NetworkUtils.Header header, ChunkRequestData body)
        {
            if (IsHost)
            {
                switch (body.State)
                {
                    case RequestState.Drop:
                        var chunks = WorldQuery.PlayersWorld[header.Author].GetChunks(body.ChunksPosition);
                        foreach (var chunk in chunks)
                            chunk.Owners.Remove(header.Author);
                        break;
                    case RequestState.Request:
                        var chunksToRequest = WorldQuery.PlayersWorld[header.Author].GetChunks(body.ChunksPosition);
                        break;
                }
            }
          
        }
    }

    public struct ChunkData : IMessageData
    {
        public Vector2Int Position;
        public byte[] Data;
    }
    public class ChunkDataTransfer : NetworkPacket<ChunkData>
    {
        public override NetworkMessageIdenfitier Identifier { get; } = NetworkMessageIdenfitier.World;
        protected override void OnPacketReceived(NetworkUtils.Header header, ChunkData body)
        {
            var chunk = WorldQuery.PlayersWorld[header.Author].AddChunk(body.Position, LZ4.Decompress(body.Data));

            if (!IsHost)
                chunk.Render();
                
        }
    }
}