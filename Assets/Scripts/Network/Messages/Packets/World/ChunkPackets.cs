using MessagePack;
using UnityEngine;
using World;
using World.Chunks;

namespace Network.Messages.Packets.World
{
    [MessagePackObject]
    public struct ChunkTransferMessage : IMessageData
    {
        [Key(0)] public byte[] Data;
        [Key(1)] public Vector2Int Position;
        [Key(3)] public WorldIdentifier World;
    }

    public class ChunkTransferPacket : NetworkPacket<ChunkTransferMessage>
    {
        public override NetworkMessageIdenfitier Identifier { get; } = NetworkMessageIdenfitier.World;

        protected override void OnPacketReceived(NetworkUtils.Header header, ChunkTransferMessage body)
        {
            var world = WorldsManager.Instance.GetWorld(body.World);
            world.ClientHandler.OnChunkReceived(body.Position, body.Data);
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
        [Key(0)] public Vector2Int[] Positions;
        [Key(1)] public ChunkRequestType Type;
        [Key(2)] public WorldIdentifier World;
    }

    public class ChunkRequestPacket : NetworkPacket<ChunkRequestMessage>
    {
        public override NetworkMessageIdenfitier Identifier { get; } = NetworkMessageIdenfitier.World;

        protected override void OnPacketReceived(NetworkUtils.Header header, ChunkRequestMessage body)
        {
            switch (body.Type)
            {
                case ChunkRequestType.Request:
                    WorldsManager.Instance.GetWorld(body.World).ServerHandler.PlayerRequestChunks(header.Author, body.Positions);
                    break;
                case ChunkRequestType.Drop:
                    WorldsManager.Instance.GetWorld(body.World).ServerHandler.PlayerDropChunks(header.Author, body.Positions);
                    break;
            }
        }
    }
}