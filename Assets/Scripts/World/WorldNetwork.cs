using System.Collections.Generic;
using System.Linq;
using MessagePack;
using Network.Messages;
using Network.Messages.Packets.World;
using Unity.Netcode;
using UnityEngine;
using World.Blocks;
using World.Chunks;

namespace World
{
    public abstract partial class AbstractWorld
    {
        
        public void OnChunkUpdated(Chunk chunk)
        {
        }

        public void SendChunkToServer(Chunk chunk)
        {
            var packet = new ChunkTransferMessage
            {
                ChunkData = ChunkUtils.SerializeAndCompressChunk(chunk),
                Center = chunk.Center,
                IsEmpty = chunk.IsEmpty,
                Source = PacketSouce.Client
            };
            MessageFactory.SendPacket(SendingMode.ClientToServer, packet, null, null, NetworkDelivery.ReliableFragmentedSequenced);
        }
        
        public void OnChunkReceived(byte[] chunkData, Vector2Int center, bool isEmpty)
        {
            Chunk chunk;
            if (isEmpty)
            {
                chunk = new Chunk(this, center);                
                WorldGeneration.WorldGeneration.GenerateChunk(this, chunk);
            }
            else
                chunk = new Chunk(this, center, ChunkUtils.DeserializeAndDecompressChunk(chunkData).BlockStates);
            Scanner.RequestedChunks.Remove(center);
            AddChunk(chunk);   
        }
    }
}