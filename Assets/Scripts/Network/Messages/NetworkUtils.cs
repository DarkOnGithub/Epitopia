using JetBrains.Annotations;
using MessagePack;
using Network.Messages;
using Unity.Netcode;

namespace Network.Packets.Packets
{
    public static class NetworkUtils
    {
        [MessagePackObject]
        public struct Header
        {
            [Key(0)]
            public short PacketId;
            [Key(1)]
            public byte SendingMode;
            [Key(2)]
            public ulong Author;
            [Key(3)]
            public ulong[] TargetIds;
        }
        /// <summary>
        /// Header format:
        ///     Author: ulong
        ///     SendingMode: byte
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="author"></param>
        /// <param name="targetIds"></param>
        /// <param name="packetId"></param>
        /// <returns></returns>=
        public static byte[] GenerateHeader(SendingMode mode, short packetId, ulong author, [CanBeNull] ulong[] targetIds = null)
		{
            return MessagePackSerializer.Serialize(new Header
                                                   {
                                                       PacketId = packetId,
                                                       SendingMode = (byte)mode,
                                                       Author = author,
                                                       TargetIds = targetIds
                                                   });
		}
        
        public static void WriteBytesToWriter(ref FastBufferWriter writer, byte[] bytes)
        {
            writer.WriteValue(bytes.Length);
            writer.WriteBytes(bytes);
        }
        public static void WriteBytesToWriterSafe(ref FastBufferWriter writer, byte[] bytes)
        {
            if (!writer.TryBeginWrite(bytes.Length + sizeof(int))) return;
            writer.WriteValue(bytes.Length);
            writer.WriteBytes(bytes);
        }
    }
}