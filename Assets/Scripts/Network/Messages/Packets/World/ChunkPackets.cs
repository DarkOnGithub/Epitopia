using MessagePack;
using Unity.Netcode;
using UnityEngine;
using World;

namespace Network.Messages.Packets.World
{
    [MessagePackObject]
    public struct ChunkTransferMessage : IMessageData
    {
        [Key(0)]
        public byte[] ChunkData;
        [Key(1)]
        public Vector2Int Center;
        [Key(2)]
        public PacketSouce Source;

        [Key(3)] public bool IsEmpty;
        [Key(4)]
        public WorldIdentifier World;
    }
    public class ChunkTransferPacket : NetworkPacket<ChunkTransferMessage>
    {
        public override NetworkMessageIdenfitier Identifier { get; } = NetworkMessageIdenfitier.World;
        protected override void OnPacketReceived(NetworkUtils.Header header, ChunkTransferMessage body)
        {
            switch (body.Source)
            {
                case PacketSouce.Client:
                    WorldManager.GetWorld(body.World).HostController.OnChunkReceived(body.ChunkData, body.Center, header.Author);
                    break;
                case PacketSouce.Server:
                    var world = WorldManager.GetWorld(body.World);
                    world.OnChunkReceived(body.ChunkData, body.Center, body.IsEmpty, world);
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
            if (!IsHost) return;
            switch (body.Type)
            {
                case ChunkRequestType.Drop:
                    foreach (var position in body.Positions)
                    {
                        if(WorldManager.GetWorld(body.World).TryGetChunk(position, out var chunk))
                        {
                            chunk.RemoveOwner(header.Author);
                        }
                    }
                    break;
                case ChunkRequestType.Request:
                    WorldManager.GetWorld(body.World).HostController.GetChunksInRange(body.Positions, header.Author);
                    break;
            }
        }
    }
}