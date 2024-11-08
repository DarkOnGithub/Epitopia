using MessagePack;
using UnityEngine;
using Utils.LZ4;
using World;
using World.WorldGeneration;

namespace Network.Messages.Packets.World
{
    public enum RequestState
    {
        Drop = 0,
        Request = 1,
    }
    [MessagePackObject]
    public struct ChunkRequestData : IMessageData
    {
        [Key(0)]
        public Vector2Int[] ChunksPosition;
        [Key(1)]
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
                        var chunks = WorldManager.PlayersWorld[header.Author].GetChunks(body.ChunksPosition);
                        foreach (var chunk in chunks)
                            chunk.RemoveOwner(header.Author);
                        break;
                    case RequestState.Request:
                        var world = WorldManager.PlayersWorld[header.Author];
                        var chunksToRequest = world.GetChunks(body.ChunksPosition);
                        foreach (var chunk in chunksToRequest)
                            world.SendChunkToClient(chunk, new []{ header.Author });
                        break;
                }
            }
          
        }
    }
    [MessagePackObject]
    public struct ChunkData : IMessageData
    {
        [Key(0)]
        public Vector2Int Position;
        [Key(1)]
        public byte[] Data;
        [Key(2)] 
        public bool IsEmpty;
    }
    public class ChunkDataTransfer : NetworkPacket<ChunkData>
    {
        public override NetworkMessageIdenfitier Identifier { get; } = NetworkMessageIdenfitier.World;
        protected override void OnPacketReceived(NetworkUtils.Header header, ChunkData body)
        {
            var chunk = WorldManager.PlayersWorld[header.Author].AddChunk(body.Position, LZ4.Decompress(body.Data));
            if (chunk.IsEmpty)
                WorldGeneration.GenerateChunk(chunk);
            Debug.Log(body.Position);
        }
    }
}