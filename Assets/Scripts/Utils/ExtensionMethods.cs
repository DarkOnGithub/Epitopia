namespace Utils
{
    public static class ExtensionMethods
    {
        public static string ToHex(this short packetId)
        {
            return "0x" + packetId.ToString("X");
        }
    }
}