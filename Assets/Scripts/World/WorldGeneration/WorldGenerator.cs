using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Network.Server;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEditor.PackageManager;
using UnityEngine;
using Utils;
using World.WorldGeneration.Biomes;
using World.WorldGeneration.Noise;
using Random = System.Random;


namespace World.WorldGeneration
{
    public struct Noise1DStruct
    {
        public string Argument;
        public float[] Splines;
    }

    public struct Noise2DStruct
    {
        public string Argument;
        public float Threshold;
    }

    public struct NoiseGeneratorStruct
    {
        public string Type;
        public string Noise;
        public float Frequency;
    }

    public struct WorldStruct
    {
        public string Name;
        public Dictionary<string, NoiseGeneratorStruct> Noises;
        public NoiseRouterStruct HeightMapRouter;
        public string[] Biomes;
        public int ElevationFactor;
    }

    public class WorldGenerator
    {
        private static int _seed = 0;

        private static int Seed
        {
            get
            {
                _seed += _randomizerSeed.Next();
                return _seed;
            }
        }

        private static Random _randomizerSeed = new();

        private const string NoisePath = "/WorldGeneration/Noises/";
        private const string WorldsPath = "/WorldGeneration/Worlds/";
        
        public int ElevationFactor;
        public Dictionary<string, NoiseGenerator> Noises = new();
        public NoiseGenerator1D Entry;
        public HeightMapRouter HeightMapRouter;
        public BiomeRouter BiomeRouter;
        public BiomeFinder BiomeFinder;
        public WorldGenerator(AbstractWorld worldIn)
        {
            var worldPath =
                Server.Instance.ConfigDirectory + WorldsPath + worldIn.Identifier.GetWorldName() + ".json";
            if (!File.Exists(worldPath)) return;
        
            var worldGenParameters = JsonConvert.DeserializeObject<WorldStruct>(File.ReadAllText(worldPath));
            var jsonContent = JObject.Parse(File.ReadAllText(worldPath));

            ElevationFactor = worldGenParameters.ElevationFactor;
            
            foreach (var noise in worldGenParameters.Noises)
            {
                var noisePath = Server.Instance.ConfigDirectory + NoisePath + noise.Value.Noise + ".json";
                if (!File.Exists(noisePath)) continue;
                switch (noise.Value.Type)
                {
                    case "Noise1D":
                        var noiseContent =
                            JsonConvert.DeserializeObject<Noise1DStruct>(File.ReadAllText(noisePath));
                        Noises.Add(noise.Key,
                                   NoiseGenerator1D.FromJson(noiseContent, noise.Value.Frequency,
                                                             Seed + _randomizerSeed.Next()));
                        break;
                    case "Noise2D":
                        var noiseContent2D =
                            JsonConvert.DeserializeObject<Noise2DStruct>(File.ReadAllText(noisePath));
                        Noises.Add(noise.Key,
                                   NoiseGenerator2D.FromJson(noiseContent2D, noise.Value.Frequency,
                                                             Seed + _randomizerSeed.Next()));
                        break;
                }
            }

            HeightMapRouter = new HeightMapRouter(worldGenParameters.HeightMapRouter, this,
                                                  jsonContent.Get<JObject>("HeightMapRouter").Get<int>("Amplitude"));
            BiomeRouter = new(new()
                              {
                                  (NoiseGenerator1D)Noises["Temperature"],
                                  (NoiseGenerator1D)Noises["Humidity"],
                                  (NoiseGenerator1D)Noises["Vegetation"]  
                              });
            BiomeFinder = new();
            foreach (var biome in worldGenParameters.Biomes)
                BiomeFinder.AddBiome(new Biome(biome));
        }
    }
}