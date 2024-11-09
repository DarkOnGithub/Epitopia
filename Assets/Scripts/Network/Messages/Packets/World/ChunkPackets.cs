using MessagePack;
using Unity.Netcode;
using UnityEngine;
using World;

namespace Network.Messages.Packets.World
{
     [MessagePackObject]
     public struct ChunkSenderMessage : IMessageData
     {
          [Key(0)]
          public Vector2Int Position;
          [Key(1)]
          public byte[] Data;
          [Key(2)]
          public WorldIdentifier World;
          [Key(3)]
          public bool IsEmpty;
     }
     public class ChunkReceiver : NetworkPacket<ChunkSenderMessage>
     {
          public override NetworkMessageIdenfitier Identifier { get; } = NetworkMessageIdenfitier.World;
          protected override void OnPacketReceived(NetworkUtils.Header header, ChunkSenderMessage body)
          {
               if(header.Author == NetworkManager.ServerClientId)
                    WorldManager.GetWorld(body.World).ReceiveChunkFromServer(body);
               else
                    WorldManager.GetWorld(body.World).ReceiveChunkFromClient(body);
          }
     }

     public enum ChunkRequestType
     {
          Request = 0,
          Drop = 1
     }
     [MessagePackObject]
     public struct ChunkRequestMessage : IMessageData
     {
          [Key(0)]
          public ChunkRequestType RequestType;
          [Key(1)]
          public Vector2Int[] Positions;
          [Key(2)]
          public WorldIdentifier World;
     }
     public class PlayerChunkRequest : NetworkPacket<ChunkRequestMessage>
     {
          public override NetworkMessageIdenfitier Identifier { get; } = NetworkMessageIdenfitier.World;
          protected override void OnPacketReceived(NetworkUtils.Header header, ChunkRequestMessage body)
          {
               switch (body.RequestType)
               {
                    case ChunkRequestType.Request:
                         if(header.Author == NetworkManager.ServerClientId)
                              WorldManager.GetWorld(body.World).RequestChunksHandler(body.Positions, new[] {header.Author});
                         break;
                    case ChunkRequestType.Drop:
                         if(header.Author == NetworkManager.ServerClientId)
                              WorldManager.GetWorld(body.World).DropChunks(body.Positions, new[] {header.Author});
                         break;
               }    
          }
     }
     
}