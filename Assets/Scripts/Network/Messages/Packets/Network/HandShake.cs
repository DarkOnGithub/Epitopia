using MessagePack;
using Players;
using UnityEngine;

namespace Network.Messages.Packets.Network
{
    [MessagePackObject]
    public struct HandShakeData : IMessageData
    {
        [Key(0)]
        public ulong ClientId { get; set; }
        [Key(1)]
        public string Username { get; set; }
        [Key(2)]
        public string PlayerId { get; set; }
    }
    public class HandShake : NetworkPacket<HandShakeData>
    {
        public override NetworkMessageIdenfitier Identifier { get; } = NetworkMessageIdenfitier.Network;
        protected override void OnPacketReceived(NetworkUtils.Header header, HandShakeData body)
        {
            Debug.Log($"Player {body.Username} connected with ClientId {body.ClientId} and PlayerId {body.PlayerId}");
            var player = new Player()
                     {
                         ClientId = body.ClientId,
                         Username = body.Username,
                         PlayerId = body.PlayerId
                     };
            PlayerManager.Players.Add(player);
            if(header.Author == NetworkHandler.ClientId)
                PlayerManager.LocalPlayer = player;
                

        }
    }
}