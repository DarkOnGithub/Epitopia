// using MessagePack;
// using Unity.Netcode;
// using UnityEngine;
// using World;
// using World.Blocks;
// using World.Chunks;
//
// namespace Network.Messages.Packets.World
// {
//  
//     public enum BlockActionType
//     {
//         Place = 0,
//         Break = 1
//     }
//     [MessagePackObject]
//     public struct BlockActionMessage : IMessageData
//     {
//         [Key(0)]public Vector2Int Position;
//         [Key(1)]public IBlockState BlockState;
//         [Key(2)]public BlockActionType Type;
//         [Key(3)]public WorldIdentifier World;
//     }
//     
//     public class BlockActionPacket : NetworkPacket<BlockActionMessage>
//     {
//         public override NetworkMessageIdenfitier Identifier { get; } = NetworkMessageIdenfitier.World;
//         protected override void OnPacketReceived(NetworkUtils.Header header, BlockActionMessage body)
//         {
//             var world = WorldManager.GetWorld(body.World);
//             var state = BlockRegistry.GetBlock(body.BlockState.Id).FromIBlockState(body.BlockState);
//             
//             state.WallId = body.BlockState.WallId;
//             if (NetworkManager.Singleton.IsClient)
//             {
//                 if (world.ClientHandler.WorldIn.Query.FindNearestChunk(body.Position, out var chunk))
//                 {
//                     switch (body.Type)
//                     {
//                         case BlockActionType.Place:
//                             chunk.PlaceBlockClient(body.Position, state);
//                             break;
//                         case BlockActionType.Break:
//                             chunk.BreakBlockClient(body.Position);
//                             break;
//                     }
//                 }
//             }
//             else
//             {
//                 Chunk chunk;
//                 switch (body.Type)
//                 {
//                     case BlockActionType.Place:
//                         if (world.ServerHandler.Query.FindNearestChunk(body.Position, out chunk))
//                             chunk.PlaceBlock(body.Position, state);
//                         break;
//                     case BlockActionType.Break:
//                         if (world.ServerHandler.Query.FindNearestChunk(body.Position, out chunk))
//                             chunk.BreakBlock(body.Position);
//                         break;
//                 }        
//             }         
//         
//         }
//     }
// }