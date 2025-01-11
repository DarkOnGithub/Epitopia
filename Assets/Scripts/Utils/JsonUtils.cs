using System.IO;
using Newtonsoft.Json.Linq;

namespace Utils
{
    public static class JsonUtils
    {
        public static T Deserialize<T>(this JObject json, string key)
        {
            return json[key].ToObject<T>();
        }

        public static T LoadJson<T>(string path)
        {
            if (!File.Exists(path)) throw new FileNotFoundException($"The file at path {path} does not exist.");

            return JObject.Parse(File.ReadAllText(path)).ToObject<T>();
        }
    }
}