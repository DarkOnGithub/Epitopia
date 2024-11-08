using JetBrains.Annotations;
using MessagePack;
using Network.Messages;
using Unity.Netcode;

namespace Network.Messages
{
    public static class NetworkUtils
    {
        [MessagePackObject]
        public struct Header
        {
            [Key(0)] public int PacketId;
            [Key(1)] public byte SendingMode;
            [Key(2)] public ulong Author;
            [Key(3)] public ulong[] TargetIds;
        }
        
        public static byte[] GenerateHeader(SendingMode mode, int packetId, ulong author,
            [CanBeNull] ulong[] targetIds = null)
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