using Newtonsoft.Json.Linq;

namespace Utils
{
    public static class ExtensionMethods
    {
        public static string ToHex(this int packetId)
        {
            return "0x" + packetId.ToString("X");
        }

        public static T Get<T>(this JObject jsonObject, string key)
        {
            return jsonObject[key].ToObject<T>();
        }
    }
}