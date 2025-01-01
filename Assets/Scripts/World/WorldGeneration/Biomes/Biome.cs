using System.IO;
using Network.Server;
using Newtonsoft.Json;
using UnityEngine;
using Utils;

namespace World.WorldGeneration.Biomes
{
    public struct BiomeDescriptor
    {
        public Range Vegetation;
        public Range Temperature;
        public Range Humidity;
        public Range Elevation;
    }
    public struct BiomeStruct
    {
        public string Name;
        public float[] Vegetation;
        public float[] Temperature;
        public float[] Humidity;
        public float[] Elevation;
        public int WaterLevel;
        public SurfaceRuleComponent[] SurfaceRules;
    }
    public class Biome
    {
        public const string BiomesPath = "/WorldGeneration/Biomes/";
        public int WaterLevel;
        public BiomeDescriptor BiomeDescriptor;
        public string BiomeName;
        public SurfaceRule SurfaceRule;
        public Biome(string biomePath)
        {
            biomePath = Server.Instance.ConfigDirectory + BiomesPath + biomePath + ".json";
            
            if (!File.Exists(biomePath))
                throw new FileNotFoundException("Biome file not found: " + biomePath);
            Debug.Log(File.ReadAllText(biomePath));
            var biomeData = JsonConvert.DeserializeObject<BiomeStruct>(File.ReadAllText(biomePath));
            BiomeDescriptor = new()
                              {
                                  Elevation = new(biomeData.Elevation[0], biomeData.Elevation[1]),
                                  Humidity = new(biomeData.Humidity[0], biomeData.Humidity[1]),
                                  Temperature = new(biomeData.Temperature[0], biomeData.Temperature[1]),
                                  Vegetation = new(biomeData.Vegetation[0], biomeData.Vegetation[1])
                              };
            WaterLevel = biomeData.WaterLevel;
            BiomeName = biomeData.Name;
            SurfaceRule = new(biomeData.SurfaceRules);
        }
    }
}