using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace World.WorldGeneration.WorldDataParser
{
    struct DataObject
    {
        public string Type { get; set; }
    }

    public static class WorldDataParser
    {
        public static T ParseData<T>(string data)
        {
            var deserialized = JsonConvert.DeserializeObject<DataObject>(data);
            return (T)TypeBinding.CallBinding(deserialized.Type, JObject.Parse(data));
        }

        public static object ParseData(JObject data)
        {
            Debug.Log(data);
            return TypeBinding.CallBinding(data["Type"].ToString(), data);
        }

        public static T Get<T>(this JObject jObject, string key)
        {
            if(jObject[key] == null)
                return default;
            return jObject[key].ToObject<T>() ;
        }
    }
}