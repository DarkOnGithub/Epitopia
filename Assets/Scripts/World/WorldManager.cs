using System;
using System.Collections;
using System.Collections.Generic;
using Players;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utils;
using World.Chunks;

namespace World
{
    public class WorldManager : NetworkBehaviour
    {
        public static Dictionary<ulong, World> PlayersWorld = new();
        public static WorldManager Instance { get; private set; }
        private const int RadiusHeight = 3;
        private const int RadiusWidth = 3;
        private const int RequestTimeoutLimit = 5;
        private const int MaxRequestSize = 20;
        private HashSet<Vector2Int> _waitingRoom = new();
        [SerializeField]
        public Tilemap tilemap;
        [SerializeField]
        public Grid grid;
        private void Awake()
        {
            Instance = this;
        }

        private void FixedUpdate()
        {
            if (PlayerManager.LocalPlayer == null)
                return;
            var playerPosition = PlayerManager.LocalPlayer.Position;
            var world = PlayerManager.LocalPlayer.World;
            var request = new List<Vector2Int>();
            for(int x = -RadiusWidth; x < RadiusWidth; x++)
            {
                for(int y = -RadiusHeight; y < RadiusHeight; y++)
                {
                    var chunkPosition = playerPosition + new Vector2Int(x, y) * AbstractChunk.ChunkSize;
                    var position = VectorUtils.GetNearestVectorDivisibleBy(chunkPosition, AbstractChunk.ChunkSize);
                    if(WorldQuery.FindNearestChunk(world, position, out var chunk))
                    {
                        chunk.Draw();
                        if(_waitingRoom.Contains(position))
                            _waitingRoom.Remove(position);
                    }
                    else if(!_waitingRoom.Contains(position))
                        request.Add(position);
                }
            }
            if(request.Count > 0)
                world.RequestChunk(request);
        }
    }
}