using System.Collections.Generic;
using System.Linq;
using Blocks;
using JetBrains.Annotations;
using MessagePack;
using Network.Messages;
using Network.Messages.Packets.World;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.XR;
using Utils;
using World.Chunks;

namespace World
{
    public abstract partial class World
    {
        public readonly Dictionary<Vector2Int, Chunk> Chunks = new();
        
        public World()
        {

        }

        [CanBeNull]
        public Chunk GetChunk(Vector2Int position)
        {
            return Chunks.GetValueOrDefault(position, null);
        }

        public List<Chunk> GetChunks(IEnumerable<Vector2Int> positions)
        {
            var chunks = new List<Chunk>();
            foreach (var position in positions)
            {
                if (!Chunks.TryGetValue(position, out var chunk))
                    chunk = GenerateChunk(position);
                
                chunks.Add(chunk);
            }
            return chunks;
        }
        public Chunk GenerateChunk(Vector2Int position)
        {
            if (Chunks.TryGetValue(position, out var _chunk))
                return _chunk;
            var chunk = new Chunk(this, position);
            Chunks.TryAdd(position, chunk);
            return chunk;
        }

        public Chunk AddChunk(Vector2Int position, byte[] content)
        {
            if (Chunks.TryGetValue(position, out var _chunk))
            {
                if(NetworkManager.Singleton.IsHost)
                    _chunk.Blocks = MessagePackSerializer.Deserialize<IBlockState[]>(content);
                Debug.Log("a");
                return _chunk;
            }
            var chunk = new Chunk(this, position, content);
            Chunks.TryAdd(position, chunk);
            return chunk;
        }

        public void RequestChunk(List<Vector2Int> positions)
        {
            MessageFactory.SendPacket(SendingMode.ClientToServer, new ChunkRequestData
            {
                ChunksPosition = positions.ToArray(),
                State = RequestState.Request
            });
            
        }
        public bool SetBlock(Vector2Int worldPosition, IBlockState block)
        {
            if (WorldQuery.SetBlock(this, worldPosition, block))
            {
               return true;       
            }
            return false;
        }
    }
}