using MessagePack;
using Unity.Netcode;
using UnityEngine;
using World;

namespace Network.Messages.Packets.World
{
    [MessagePackObject]
    public struct ChunkTransferMessage : IMessageData
    {
        [Key(0)] public byte[] ChunkData;
        [Key(1)] public Vector2Int Center;
        [Key(2)] public PacketSource Source;
        [Key(3)] public bool IsEmpty;
        [Key(4)] public WorldIdentifier World;
    }
    public class ChunkTransferPacket : NetworkPacket<ChunkTransferMessage>
    {
        public override NetworkMessageIdenfitier Identifier { get; } = NetworkMessageIdenfitier.World;
        protected override void OnPacketReceived(NetworkUtils.Header header, ChunkTransferMessage body)
        {
            var world = WorldManager.GetWorld(body.World);
            switch (body.Source)
            {
                case PacketSource.Server:
                    world.ClientHandler.OnPacketReceived(header, body);
                    break;
                case PacketSource.Client:
                    world.ServerHandler.OnPacketReceived(header, body);
                    break;
            }
        }
    }

    public enum ChunkRequestType
    {
        Drop = 0,
        Request = 1
    }

    [MessagePackObject]
    public struct ChunkRequestMessage : IMessageData
    {
        [Key(0)]
        public Vector2Int[] Positions;
        [Key(1)]
        public ChunkRequestType Type;
        [Key(2)]
        public WorldIdentifier World;
        
    }
    public class ChunkRequestPacket : NetworkPacket<ChunkRequestMessage>
    {
        public override NetworkMessageIdenfitier Identifier { get; } = NetworkMessageIdenfitier.World;
        protected override void OnPacketReceived(NetworkUtils.Header header, ChunkRequestMessage body)
        {
            switch (body.Type)
            {
                case ChunkRequestType.Request:
                    WorldManager.GetWorld(body.World).ServerHandler.PlayerRequestChunks(header.Author, body.Positions);
                    break;
                case ChunkRequestType.Drop:
                    var handler = WorldManager.GetWorld(body.World).ServerHandler;
                    foreach (var chunk in handler.Query.LazyGetChunks(body.Positions))
                        handler.RemovePlayerFromChunk(chunk, header.Author);
                    break;                    
            }
        }
    }
}