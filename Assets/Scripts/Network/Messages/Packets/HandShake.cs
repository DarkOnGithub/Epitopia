// using MessagePack;
// using UnityEngine;
//
// namespace Network.Messages.Packets
// {
//     [MessagePackObject]
//     public struct HandshakeMessage : IMessageData
//     {
//         [Key(0)] public string Message;
//     }
//     public class HandShake : NetworkPacket <HandshakeMessage>
//     {
//         public override NetworkMessageIdenfitier Identifier { get; } = NetworkMessageIdenfitier.Network;
//         protected override void OnPacketReceived(NetworkUtils.Header header, HandshakeMessage body)
//         {
//             Debug.Log($"User: {header.Author}, Says: {body.Message}");
//         }
//         
//         public static void SendPacket(string message)
//         {
//             MessageFactory.SendPacket(SendingMode.ClientToClient, new HandshakeMessage
//             {
//                 Message = message
//             });
//         }
//     }
// }

