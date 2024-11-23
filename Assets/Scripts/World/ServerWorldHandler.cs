using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Network.Messages;
using Network.Messages.Packets.World;
using PimDeWitte.UnityMainThreadDispatcher;
using Storage;
using Unity.VisualScripting;
using UnityEngine;
using World.Chunks;

namespace World
{
    public class ServerWorldHandler : IWorldHandler, IDisposable
    {
        private static BetterLogger _logger = new BetterLogger(typeof(ServerWorldHandler));
        public AbstractWorld WorldIn { get; }
        private Dictionary<Vector2Int, Chunk> _chunks = new();
        public WorldStorage Storage;
        public WorldQuery Query;
        private bool _disposed = false;
        public ServerWorldHandler(AbstractWorld worldIn)
        {
            WorldIn = worldIn;
            Storage = new WorldStorage(worldIn.Identifier.GetWorldName());
            Query = new WorldQuery(worldIn, _chunks);
        }
        
        public void OnPacketReceived(NetworkUtils.Header header, ChunkTransferMessage message)
        {
            if(Query.TryGetChunk(message.Center, out var chunk))
            {
                if ((chunk.IsEmpty && chunk.ChunkGeneratorPlayerId == header.Author) | !chunk.IsEmpty)
                {
                    var content = ChunkUtils.DeserializeChunk(message.ChunkData).BlockStates;
                    chunk.UpdateContent(content);
                    var c = 0;
                    foreach (var VARIABLE in chunk.BlockStates)
                    {
                        if (VARIABLE.Id == 2)
                            c++;
                    }

                    Debug.Log(c);
                    chunk.IsEmpty = false;
                    var players = chunk.Players
                                       .Where(plr => plr != header.Author)
                                       .ToArray();
                    if(players.Length > 0)
                        SendChunk(chunk, players);
                }
            }
        }

        public void SendChunk(Chunk chunk, ulong[] clients) => 
            UnityMainThreadDispatcher.Instance().Enqueue(() => MessageFactory.SendPacket(SendingMode.ServerToClient,
            new ChunkTransferMessage
            {
                ChunkData = ChunkUtils.SerializeChunk(chunk),
                Center = chunk.Center,
                IsEmpty = chunk.IsEmpty,
                Source = PacketSource.Server 
            }, clients
        ));

        public void RemoveChunk(Chunk chunk)
        {
            
        }

        public void PlayerRequestChunks(ulong player, Vector2Int[] position)
        {
            foreach (var chunk in LoadChunksInRange(position))
            {
                //!TODO handle the case where the first owner left while loading the chunk, the chunk will still be loaded but no one will be able to generate it
                //!TODO maybe not because when the player leave, the player is auto removed from the chunk, so the chunk will be destroyed ? maybe pass the loading to the next player
                if(chunk.IsEmpty)
                    chunk.ChunkGeneratorPlayerId = player;
                AddPlayerToChunk(chunk, player);
                SendChunk(chunk, new []{ player });
            }
            
        }

        public void AddPlayerToChunk(Chunk chunk, ulong player)
        {
            chunk.Players.Add(player);
        }
        
        public void RemovePlayerFromChunk(Chunk chunk, ulong player)
        {

            chunk.Players.Remove(player);
            if (chunk.Players.Count == 0)
                DestroyChunk(chunk);
        }

        private void DestroyChunk(Chunk chunk)
        {
            Query.RemoveChunk(chunk.Center);
            Storage.Put(chunk.Center, ChunkUtils.SerializeChunk(chunk, false));
            chunk.Dispose();
        }
        
        private bool GetChunkFromStorage(Vector2Int position, out Chunk chunk)
        {
            if (Storage.TryGet(position, out var bytes))
            {
                // chunk = Query.CreateChunk(position, ChunkUtils.DeserializeChunk(bytes, false).BlockStates);
                // return true;
            }
            chunk = null;
            return false;
        }

        public bool GetChunkFromMemoryOrStorage(Vector2Int position, out Chunk chunk) =>
            Query.TryGetChunk(position, out chunk) || GetChunkFromStorage(position, out chunk);
        
        //Lazy loading
        public IEnumerable<Chunk> LoadChunksInRange(Vector2Int[] positions)
        {
            foreach (var position in positions)
            {
                if (GetChunkFromMemoryOrStorage(position, out var chunk))
                    yield return chunk;                 
                else 
                    yield return Query.CreateEmptyChunk(position);
            }            
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
            {
                foreach (var chunk in _chunks.Values)
                    DestroyChunk(chunk);
                _chunks = null;
                Storage.Close();
            }
            _disposed = true;
        }

        ~ServerWorldHandler()
        {
            Dispose(false);
        }
    }
}