// using System;
// using System.Collections.Generic;
// using System.Linq;
// using Core.Lightning;
// using JetBrains.Annotations;
// using MessagePack;
// using Network.Messages;
// using Network.Messages.Packets.World;
// using Renderer;
// using Unity.Netcode;
// using UnityEngine;
// using UnityEngine.UI;
// using Utils;
// using World.Blocks;
//
// namespace World.Chunks
// {
//     [MessagePackObject]
//     public struct ChunkData
//     {
//         [Key(0)] public Vector2Int Center { get; set; }
//         [Key(1)] public IBlockState[] BlockStates { get; set; }
//         [Key(2)] [CanBeNull] public Dictionary<Vector2Int, (IBlockState, bool)> BlockToPlace { get; set; }
//     }
//
//     public class Chunk : IDisposable
//     {
//         public const int ChunkSize = 16;
//         public const int ChunkSizeSquared = ChunkSize * ChunkSize;
//         public readonly AbstractWorld World;
//         private bool _disposed;
//         private bool _isRendered;
//         public Dictionary<Vector2Int, int> LightSources = new();
//         public Dictionary<Vector2Int, (IBlockState, bool)> BlockToPlace { get; set; }
//         public IBlockState[] BlockStates = new IBlockState[ChunkSizeSquared];
//         public HashSet<ulong> Players = new();
//         public Texture2D LightTexture;
//         public GameObject LightmapObject;
//         public bool IsEmpty { get; set; } = true;
//
//         public Chunk(AbstractWorld worldIn, Vector2Int center)
//         {
//             World = worldIn ?? throw new ArgumentNullException(nameof(worldIn));
//             UpdateContent(new IBlockState[ChunkSizeSquared]);
//             Center = center;
//
//         }
//
//         public Chunk(AbstractWorld worldIn, Vector2Int center, IBlockState[] states)
//         {
//             World = worldIn ?? throw new ArgumentNullException(nameof(worldIn));
//             UpdateContent(states);
//             Center = center;
//             IsEmpty = false;
//         }
//
//         public Vector2Int Center { get; }
//
//         public Vector2Int Origin
//         {
//             get
//             {
//                 var half = ChunkSize / 2;
//                 return new Vector2Int(Center.x - half, Center.y - half);
//             }
//         }
//
//         
//         public void UpdateContent(IBlockState[] newContent)
//         {
//             var air = BlockRegistry.BLOCK_AIR.GetDefaultState();
//             var world = World;
//             var ignore = new HashSet<Vector2Int>();
//             for (var i = 0; i < ChunkSizeSquared; i++)
//             {
//                 var position = i.ToVector2Int() + Origin;
//                 if(world.ServerHandler.BlocksToLoad.TryGetValue(position, out var block))
//                 {
//                     newContent[i] = block;
//                     continue;
//                 }
//                 var newState = newContent[i];
//
//
//                 if (newState == null)
//                     BlockStates[i] = air;
//                 else
//                 {
//                     if(newState.Block is TreeBlock)
//                     {
//                         var treeBlock = (TreeBlock)(newState.Block);
//                         
//                         treeBlock.Place(BlockStates, position, ((TreeBlockState)newState).Height, ignore, world);
//                         continue;
//                     }
//                     BlockStates[i] = BlockRegistry.GetBlock(newState.Id).FromIBlockState(newState);
//                     BlockStates[i].LightLevel = newState.LightLevel;
//                     BlockStates[i].WallId = newState.WallId;
//                 }
//             }
//         }
//
//
//         public void PlaceBlockClient(Vector2Int position, IBlockState blockState)
//         {
//             var index = VectorUtils.WorldPositionToLocalPosition(position, Origin).ToIndex();
//             SetBlockSafe(index, blockState);
//             _isRendered = false;
//         }
//         
//         public void BreakBlockClient(Vector2Int position)
//         {
//             var index = VectorUtils.WorldPositionToLocalPosition(position, Origin).ToIndex();
//             RemoveBlockSafe(index);
//             _isRendered = false;
//         }
//         public void PlaceBlock(Vector2Int position, IBlockState blockState, bool update = true)
//         {
//             var index = VectorUtils.WorldPositionToLocalPosition(position, Origin).ToIndex();
//             SetBlockSafe(index, blockState);
//             MessageFactory.SendPacket(SendingMode.ClientToServer, new BlockActionMessage
//                                                                     {
//                                                                         Position = position,
//                                                                         BlockState = blockState,
//                                                                         World = World.Identifier,
//                                                                         Type = BlockActionType.Place
//                                                                     });
//         }
//         public void PlaceBlocks(Dictionary<Vector2Int, IBlockState> blocks, bool update = true)
//         {
//             foreach (var (position, blockState) in blocks)
//             {
//                 var index = VectorUtils.WorldPositionToLocalPosition(position, Origin).ToIndex();
//                 SetBlockSafe(index, blockState);
//                 MessageFactory.SendPacket(SendingMode.ClientToServer, new BlockActionMessage
//                                                                       {
//                                                                           Position = position,
//                                                                           World = World.Identifier,
//                                                                           Type = BlockActionType.Break
//                                                                       });
//             }
//          
//         }
//         
//         
//         public void BreakBlock(Vector2Int position, bool update = true)
//         {
//             var index = VectorUtils.WorldPositionToLocalPosition(position, Origin).ToIndex();
//             RemoveBlockSafe(index);
//             if (update)
//                 WorldManager.ChunkSenderQueue.Enqueue(this);
//         }
//         
//         public void BreakBlocks(IEnumerable<Vector2Int> positions, bool update = true)
//         {
//             foreach (var position in positions)
//             {
//                 var index = VectorUtils.WorldPositionToLocalPosition(position, Origin).ToIndex();
//                 RemoveBlockSafe(index);
//             }
//             if (update)
//                 WorldManager.ChunkSenderQueue.Enqueue(this);
//         }
//         
//         public bool IsWithinChunk(Vector2Int position)
//         {
//             var origin = Origin;
//             return position.x >= origin.x && position.x < origin.x + ChunkSize &&
//                    position.y >= origin.y && position.y < origin.y + ChunkSize;
//         }
//         public void AddLightSource(Vector2Int worldPosition, int source) 
//             => LightSources[VectorUtils.WorldPositionToLocalPosition(worldPosition, Origin)] = source;
//         
//         public void RemoveLightSource(Vector2Int worldPosition) 
//             => LightSources.Remove(VectorUtils.WorldPositionToLocalPosition(worldPosition, Origin));
//         public T GetBlock<T>(int index) where T : IBlockState
//         {
//             ValidateIndex(index);
//             return (T)BlockStates[index];
//         }
//
//         public IBlockState GetBlock(int index)
//         {
//             ValidateIndex(index);
//             return BlockStates[index];
//         }
//
//         public void SetBlock(int index, IBlockState state)
//         {
//             ValidateIndex(index);
//             BlockStates[index] = state;
//         }
//
//         public void RemoveBlock(int index)
//         {
//             ValidateIndex(index);
//             BlockStates[index] = null;
//         }
//
//         public T GetBlockSafe<T>(int index) where T : IBlockState  => IsValidIndex(index) ? (T)BlockStates[index] : default;
//         
//
//         public IBlockState GetBlockSafe(int index) => IsValidIndex(index) ? BlockStates[index] : null;
//         
//
//         public void SetBlockSafe(int index, IBlockState state)
//         {
//             if (IsValidIndex(index)) BlockStates[index] = state;
//         }
//
//         public void RemoveBlockSafe(int index)
//         {
//             if (IsValidIndex(index)) BlockStates[index] = null;
//         }
//
//         public ChunkData GetChunkData() => new ChunkData
//             {
//                 Center = Center,
//                 BlockStates = BlockStates,
//                 BlockToPlace = (BlockToPlace == null || BlockToPlace.Count == 0) ? null : BlockToPlace,
//             };
//         
//         
//
//
//         private static bool IsValidIndex(int index) => index >= 0 && index < ChunkSizeSquared;
//         
//
//         private static void ValidateIndex(int index)
//         {
//             if (!IsValidIndex(index))
//                 throw new ArgumentOutOfRangeException(nameof(index),
//                                                       $"Index must be between 0 and {ChunkSizeSquared - 1}");
//         }
//
//         public void Render()
//         {
//             if (_disposed) return;
//             if (_isRendered) return;
//             _isRendered = true;
//             ChunkRenderer.RenderChunk(this);
//         }
//
//         public void UnRender()
//          => ChunkRenderer.UnRenderChunk(this);
//         
//
//         public void Destroy()
//         {
//             if (_isRendered)
//                 UnRender();
//             World.ClientHandler.RemoveChunk(this);
//             GameObject.Destroy(LightmapObject);
//
//         }
//
//         protected virtual void Dispose(bool disposing)
//         {
//             if (_disposed)
//                 return;
//
//             Destroy();
//
//             if (disposing)
//             {
//                 Players.Clear();
//                 BlockStates = null;
//             }
//
//
//             _disposed = true;
//         }
//
//         ~Chunk()
//         {
//             Dispose(false);
//         }
//         public void Dispose()
//         {
//             Dispose(true);
//             GC.SuppressFinalize(this);
//         }
//
//     }
// }

using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using MessagePack;
using MessagePack.Resolvers;
using Renderer;
using Unity.Netcode;
using UnityEngine;
using Utils;
using World.Blocks;

namespace World.Chunks
{
    public class Chunk
    {
        private static readonly MessagePackSerializerOptions SerializationOptions =
            MessagePackSerializerOptions.Standard
                .WithCompression(MessagePackCompression.Lz4BlockArray);
        
        public const int ChunkSize = 16;
        public const int ChunkSizeSquared = ChunkSize * ChunkSize;
        public readonly AbstractWorld WorldIn;
        public Vector2Int Position;
        public Vector2Int Center => Position + new Vector2Int(ChunkSize / 2, ChunkSize / 2);
        public HashSet<ulong> Players = new();

        private bool _shouldRender = true;
        public event EventHandler OnBlockPlaced;
        
        private readonly IBlockState[] _blockStates = new IBlockState[ChunkSizeSquared];
        private static bool IsServerHost => NetworkManager.Singleton.IsHost;
        private readonly ServerWorldHandler _serverHandler;
        
        private Texture2D _lightTexture;
        public IBlockState[] BlockStates
        {
            get
            {
                var buffer = new IBlockState[ChunkSizeSquared];
                Array.Copy(_blockStates, buffer, ChunkSizeSquared);
                return buffer;
            }
        }

        public IBlockState[] BlockStatesRef => _blockStates;
        
        public bool IsEmpty
        {
            get { return _blockStates.All((state) => state.Id == 0); }
        }

        
        public Chunk(AbstractWorld worldIn, Vector2Int position)
        {
            WorldIn = worldIn;
            Position = position;
            _serverHandler = worldIn.ServerHandler;
            for (var i = 0; i < ChunkSizeSquared; i++)
                _blockStates[i] = BlockRegistry.BlockAir.CreateBlockState();
            if(IsServerHost)
            {
                UpdateContent();
                worldIn.ServerHandler.OnTick += OnTick;
            }
        }

        public Chunk(AbstractWorld worldIn, Vector2Int position, IBlockState[] blockStates)
        {
            WorldIn = worldIn;
            Position = position;
            _blockStates = blockStates;
            _serverHandler = worldIn.ServerHandler;

            if(IsServerHost)
            {
                UpdateContent();
                worldIn.ServerHandler.OnTick += OnTick;
            }
        }

        private void UpdateContent()
        {
            if (WorldIn.ServerHandler.BlocksToPlace.TryGetValue(Position, out var blocks))
            {
                lock (blocks) 
                {
                    foreach (var (index, blockState, replace) in blocks)
                        TryPlaceBlockAt(blockState, index, false, replace);
                    blocks.Clear(); 
                }
            }
            Update();
        }

        public void UpdateContent(IBlockState[] newContent)
        {
            for (var i = 0; i < ChunkSizeSquared; i++)
                _blockStates[i] = newContent[i];
            Update();
        }
        
        public IBlockState GetBlockAt(int index)
        {
            return _blockStates[index];
        }

        public void SetBlockAt(int index, IBlockState blockState)
        {
            // var localPosition = index.ToVector2Int();
            // var x = Position.x + localPosition.x;
            // var y = Position.y + localPosition.y;
            // if (IsServerHost && !BlockRegistry.GetBlock(blockState.Id).Properties.IsTransparent)
            // {
            //     if (_serverHandler.HeightsPerColumn.TryGetValue(x, out var heights))
            //         heights.TryAdd(y, 0);
            //     else
            //     {
            //         _serverHandler.HeightsPerColumn[x] = new();
            //         _serverHandler.HeightsPerColumn[x].TryAdd(y, 0);
            //     }
            // }
            _blockStates[index] = blockState;
        }
        public void RemoveBlockAt(int index)
        {
            // var localPosition = index.ToVector2Int();
            // var x = Position.x + localPosition.x;
            // var y = Position.y + localPosition.y;
            // if(_blockStates[index].Id != 0 && IsServerHost)
            //     if (_serverHandler.HeightsPerColumn.TryGetValue(x, out var heights))
            //         heights.TryRemove(y, out var _);

            _blockStates[index] = BlockRegistry.BlockAir.CreateBlockState(null);
        }

        public void TryPlaceBlockAt(IBlockState blockState, int index, bool update = false, bool replace = false)
        {
            if(_blockStates[index].Id != 0 && !replace) return;
            SetBlockAt(index, blockState);
            if(update)
                Update();
            //OnBlockPlaced?.Invoke(this, EventArgs.Empty);
        }
            
        
        private void OnPlayerUpdate(Action action)
        {
            action();

            if (Players.Count == 0)
                Destroy();
        }

        public void AddPlayer(ulong player)
        {
            OnPlayerUpdate(() => Players.Add(player));
        }

        public void RemovePlayer(ulong player)
        {
            OnPlayerUpdate(() => Players.Remove(player));
        }

        private void OnTick(object sender, EventArgs e)
        {
            
        }
        
        public void Update()
        {
            _shouldRender = true;
        }
        
        private void InitializeLighting()
        {
            _lightTexture = new Texture2D(ChunkSize, ChunkSize);
            var lightMapSource = Resources.Load<GameObject>("Prefabs/Lightmap");
            lightMapSource.transform.position = new Vector3(Position.x, Position.y);
            var lightMap = GameObject.Instantiate(lightMapSource);
            lightMap.GetComponent<SpriteRenderer>().material.SetTexture("_Lightmap", _lightTexture);
            _lightTexture.filterMode = FilterMode.Point;
            _lightTexture.wrapMode = TextureWrapMode.Clamp;
            _lightTexture.Apply();
        }
        
        private void ComputeLightMap()
        {
            for (var x = 0; x < ChunkSize; x++)
            {
                for (var y = 0; y < ChunkSize; y++)
                {
                    var block = _blockStates[(x, y).ToIndex()];
                    _lightTexture.SetPixel(x, y, new Color(0, 0, 0, 1 - block.LightLevel / 15f));
                }
            }
            _lightTexture.Apply();
        }
        
        public void Render()
        {
            if (!_shouldRender) return;
            if (_lightTexture == null)
                InitializeLighting();
            ComputeLightMap();
            ChunkRenderer.RenderChunk(this);
            _shouldRender = false;
        }
        
        
        public byte[] Serialize()
        {
            return MessagePackSerializer.Serialize(_blockStates);
        }

        public static Chunk Deserialize(AbstractWorld worldIn, Vector2Int position, byte[] data)
        {
            return new Chunk(
                worldIn, position,
                MessagePackSerializer.Deserialize<IBlockState[]>(data)
            );
        }
        
        

        public void Save()
        {
        }

        public void Destroy()
        {
            Save();
        }
    }
}