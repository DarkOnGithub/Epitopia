using System;
using System.IO;
using Mono.CSharp;
using Network.Server;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using UnityEngine;
using Utils;
using World.WorldGeneration.Structures;
using World.WorldGeneration.Vegetation;

namespace World.WorldGeneration.Biomes
{
    
    public struct BiomeData
    {
        public string Name { get; set; }
        public SurfaceRulesData[] SurfaceRules { get; set; }
        public VegetationComponentData[] Vegetation { get; set; }
    }
    
    public class Biome
    {
        public string BiomesPath => Server.Instance.ConfigDirectory + "/WorldGen/" + "Biomes/";

        public string Name;
        public SurfaceRules SurfaceRules;
        public StructurePlacement StructurePlacement;
        public BiomeVegetation Vegetation;
        
        public Biome(string name)
        {
            Name = name;
            var biomeData = JsonUtils.LoadJson<BiomeData>(BiomesPath + name + ".json");
            var jsonText = File.ReadAllText(BiomesPath + name + ".json");
            var jObject = JObject.Parse(jsonText);
            SurfaceRules = new(biomeData.SurfaceRules, jObject["SurfaceRules"].ToObject<JArray>());
            Vegetation = new(biomeData.Vegetation);
            // StructurePlacement = new(new[]
            // {
            //     new FlowerVegetation(new()
            //     {
            //         BlockName = "Grass",
            //         MaxSize = 4,
            //         Probability = 1f,
            //         Range = new[] { 0, 1 }
            //     })
            // }, Array.Empty<IStructure>());
        }
    }
}