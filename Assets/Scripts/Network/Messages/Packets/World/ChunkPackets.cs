using Unity.Netcode;
using UnityEngine;
using World;

namespace Network.Messages.Packets.World
{
     public struct ChunkSenderMessage : IMessageData
     {
          public Vector2Int Position;
          public byte[] Data;
          public WorldIdentifier World;
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
     public struct ChunkRequestMessage : IMessageData
     {
          public ChunkRequestType RequestType;
          public Vector2Int[] Positions;
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
                              WorldManager.GetWorld(body.World).RequestChunks(body.Positions, new[] {header.Author});
                         break;
                    case ChunkRequestType.Drop:
                         break;
               }    
          }
     }
     
}