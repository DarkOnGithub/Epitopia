using System.IO;
using Network.Server;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using UnityEngine;
using Utils;

namespace World.WorldGeneration.Biomes
{
    
    public struct BiomeData
    {
        public string Name { get; set; }
        public SurfaceRulesData[] SurfaceRules { get; set; }
    }
    
    public class Biome
    {
        public string BiomesPath => Server.Instance.ConfigDirectory + "/WorldGen/" + "Biomes/";

        public string Name;
        public SurfaceRules SurfaceRules;
        public Biome(string name)
        {
            Name = name;
            var biomeData = JsonUtils.LoadJson<BiomeData>(BiomesPath + name + ".json");
            var jsonText = File.ReadAllText(BiomesPath + name + ".json");
            var jObject = JObject.Parse(jsonText);
            SurfaceRules = new(biomeData.SurfaceRules, jObject["SurfaceRules"].ToObject<JArray>());
        }
    }
}