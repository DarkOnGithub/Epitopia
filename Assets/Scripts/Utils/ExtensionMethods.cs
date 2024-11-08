namespace Utils
{
    public static class ExtensionMethods
    {
        public static string ToHex(this int packetId)
        {
            return "0x" + packetId.ToString("X");
        }
    }
}