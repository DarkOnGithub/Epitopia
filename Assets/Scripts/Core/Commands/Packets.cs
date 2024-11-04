using Network.Packets;
using QFSW.QC;
using Unity.Netcode;

namespace Core.Commands
{
    public class Packets
    {
        [Command]
        public static void SendPacket(string text)
        {
            var packet = new TextPacket {Text = text};
            PacketFactory.SendPacket(packet);
        }
    }
}