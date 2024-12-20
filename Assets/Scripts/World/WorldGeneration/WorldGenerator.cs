using System;
using System.Collections.Generic;
using System.IO;
using Network.Server;
using Newtonsoft.Json;
using UnityEngine;
using World.WorldGeneration.Noise;


namespace World.WorldGeneration
{
    public class WorldGenerator
    {
        private struct Noise
        {
            public string Name;
            public string[] Splines;
        }

        public struct NoiseGenerator
        {
            public string Type;
            public string Noise;
            public float Frequency;
        }

        public struct World
        {
            public string Name;
            public Dictionary<string, NoiseGenerator> Noises;
            public string[] Biomes;
        }
        
        public List<NoiseGenerator> Noises = new();
        public NoiseGenerator Entry;
        private const string NoisePath = "/WorldGeneration/Noises/";
        private const string WorldsPath = "/WorldGeneration/Worlds/";
        
        
        public WorldGenerator(AbstractWorld worldIn)
        {
            var worldPath =
                (Server.Instance.ConfigDirectory + WorldsPath + worldIn.Identifier.GetWorldName() + ".json");
            Debug.Log(worldPath);
            if (File.Exists(worldPath))
            {
                Debug.Log(File.ReadAllText(worldPath));
                var jsonContent = JsonConvert.DeserializeObject<World>(File.ReadAllText(worldPath));
                Debug.Log(jsonContent.ToString());
                
                foreach (var generator in jsonContent.Noises)
                {
                    Debug.Log(generator);
                }
            }
        }
    }
}