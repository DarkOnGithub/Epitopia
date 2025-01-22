using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using QFSW.QC;
using UnityEngine;
using UnityEngine.XR;
using Utils;
using World.WorldGeneration;
using World.WorldGeneration.DensityFunction;

namespace Test.WorldGen
{
    public static class DistributionPreview
    {
        [Command]
        public static async void GenerateDistributionPreview(int startX, int startY, int width, int height)
        {
            if (Seed.Initialized == false)
                Seed.Initialize();
            var heightMap =
                new HeightMap(JsonUtils.LoadJson<HeightMapData>(
                                  Application.streamingAssetsPath +
                                  "/Config/WorldGen/DensityFunctions/HeightMap.json"));
            var cache = heightMap.CacheHeight(new Vector2Int(startX, startY), new Vector2Int(width, height));
            var field = cache.GetType()
                             .GetField("_cache", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var cacheValue = (float[])field?.GetValue(cache);
            var buffer = new float[cacheValue.Length];
            for (var i = 0; i < cacheValue.Length; i++)
                buffer[i] = (int)cache.GetPoint(i);

            using (var client = new HttpClient())
            {
                // Define the two JSON arrays
                var payload = new
                              {
                                  array1 = buffer,
                                  array2 = Enumerable.Range(0, cacheValue.Length).ToArray()
                              };

                var json = JsonConvert.SerializeObject(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("http://localhost:5000/process_arrays", content);

                var responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response: {responseString}");
            }
        }

        // [Command]
        // public static async void ContinentPreview(int startX, int startY, int width, int height)
        // {
        //     if (Seed.Initialized == false)
        //         Seed.Initialize();
        //     var heightMap =
        //         new HeightMap(JsonUtils.LoadJson<HeightMapData>(
        //                           Application.streamingAssetsPath +
        //                           "/Config/WorldGen/DensityFunctions/HeightMap.json"));
        //     var cache = heightMap.NoiseGenerators[0]
        //                          .GenerateCache(new Vector2Int(startX, startY), new Vector2Int(width, height));
        //     // var field = cache.GetType().GetField("_cache", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        //     // var cacheValue = (float[])field?.GetValue(cache);
        //     // var buffer = new float[cacheValue.Length];
        //     // for (var i = 0; i < cacheValue.Length; i++)
        //     // {
        //     //     buffer[i] = (int)cache.GetPoint(i) - (int)(heightMap.Data.higherAmplitude * (1 - 0.7));
        //     // }
        //
        //     using (var client = new HttpClient())
        //     {
        //         // Define the two JSON arrays
        //         var payload = new
        //                       {
        //                           array1 = cache,
        //                           array2 = Enumerable.Range(0, cache.Length).ToArray()
        //                       };
        //
        //         var json = JsonConvert.SerializeObject(payload);
        //         var content = new StringContent(json, Encoding.UTF8, "application/json");
        //
        //         var response = await client.PostAsync("http://localhost:5000/process_arrays", content);
        //
        //         var responseString = await response.Content.ReadAsStringAsync();
        //         Console.WriteLine($"Response: {responseString}");
        //     }
        // }
        //
        // [Command]
        // public static async void ErosionPreview(int startX, int startY, int width, int height)
        // {
        //     if (Seed.Initialized == false)
        //         Seed.Initialize();
        //     var heightMap =
        //         new HeightMap(JsonUtils.LoadJson<HeightMapData>(
        //                           Application.streamingAssetsPath +
        //                           "/Config/WorldGen/DensityFunctions/HeightMap.json"));
        //     var cache = heightMap.NoiseGenerators[1]
        //                          .GenerateCache(new Vector2Int(startX, startY), new Vector2Int(width, height));
        //     // var field = cache.GetType().GetField("_cache", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        //     // var cacheValue = (float[])field?.GetValue(cache);
        //     // var buffer = new float[cacheValue.Length];
        //     // for (var i = 0; i < cacheValue.Length; i++)
        //     // {
        //     //     buffer[i] = (int)cache.GetPoint(i) - (int)(heightMap.Data.higherAmplitude * (1 - 0.7));
        //     // }
        //
        //     using (var client = new HttpClient())
        //     {
        //         // Define the two JSON arrays
        //         var payload = new
        //                       {
        //                           array1 = cache,
        //                           array2 = Enumerable.Range(0, cache.Length).ToArray()
        //                       };
        //
        //         var json = JsonConvert.SerializeObject(payload);
        //         var content = new StringContent(json, Encoding.UTF8, "application/json");
        //
        //         var response = await client.PostAsync("http://localhost:5000/process_arrays", content);
        //
        //         var responseString = await response.Content.ReadAsStringAsync();
        //         Console.WriteLine($"Response: {responseString}");
        //     }
        // }

        [Command]
        public static async void PreviewNoise(int startX, int startY, int width, int height)
        {
            if (Seed.Initialized == false)
                Seed.Initialize();
            var heightMap =
                new HeightMap(JsonUtils.LoadJson<HeightMapData>(
                                  Application.streamingAssetsPath +
                                  "/Config/WorldGen/DensityFunctions/HeightMap.json"));
            var cache = heightMap.CacheHeight(new Vector2Int(startX, startY), new Vector2Int(width, height));
            var field = cache.GetType()
                             .GetField("_cache", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var cacheValue = (float[])field?.GetValue(cache);
            var buffer = new float[cacheValue.Length];
            for (var i = 0; i < cacheValue.Length; i++) buffer[i] = cacheValue[i];

            using (var client = new HttpClient())
            {
                // Define the two JSON arrays
                var payload = new
                              {
                                  array1 = buffer,
                                  array2 = Enumerable.Range(0, cacheValue.Length).ToArray()
                              };

                var json = JsonConvert.SerializeObject(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("http://localhost:5000/process_arrays", content);

                var responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response: {responseString}");
            }
        }

        // [Command]
        // public static async void PreviewCheese(int startX, int startY, int width, int height)
        // {
        //     if (Seed.Initialized == false)
        //         Seed.Initialize();
        //     var carver =
        //         new Carver(JsonUtils.LoadJson<CarverData>(Application.streamingAssetsPath +
        //                                                   "/Config/WorldGen/DensityFunctions/Carver.json"));
        //     var cheese = carver.CheeseCave.GenerateCache(new Vector2Int(startX, startY), new Vector2Int(width, height));
        //     // var cache = carver.CacheHeight(new Vector2Int(startX, startY), new Vector2Int(width, height));
        //     // var field = cache.GetType().GetField("_cache", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        //     // var cacheValue = (float[])field?.GetValue(cache);
        //
        //
        //     using (var client = new HttpClient())
        //     {
        //         // Define the two JSON arrays
        //         var payload = new
        //                       {
        //                           array1 = cheese,
        //                           array2 = Enumerable.Range(0, cheese.Length).ToArray()
        //                       };
        //
        //         var json = JsonConvert.SerializeObject(payload);
        //         var content = new StringContent(json, Encoding.UTF8, "application/json");
        //
        //         var response = await client.PostAsync("http://localhost:5000/process_arrays", content);
        //
        //         var responseString = await response.Content.ReadAsStringAsync();
        //         Console.WriteLine($"Response: {responseString}");
        //     }
        // }
        //
        // [Command]
        // public static async void PreviewSpag(int startX, int startY, int width, int height)
        // {
        //     if (Seed.Initialized == false)
        //         Seed.Initialize();
        //     var carver =
        //         new Carver(JsonUtils.LoadJson<CarverData>(Application.streamingAssetsPath +
        //                                                   "/Config/WorldGen/DensityFunctions/Carver.json"));
        //     var spaghettiCache =
        //         carver.SpaghettiCave.GenerateCache(new Vector2Int(startX, startY), new Vector2Int(width, height));
        //
        //
        //     using (var client = new HttpClient())
        //     {
        //         // Define the two JSON arrays
        //         var payload = new
        //                       {
        //                           array1 = spaghettiCache,
        //                           array2 = Enumerable.Range(0, spaghettiCache.Length).ToArray()
        //                       };
        //
        //         var json = JsonConvert.SerializeObject(payload);
        //         var content = new StringContent(json, Encoding.UTF8, "application/json");
        //
        //         var response = await client.PostAsync("http://localhost:5000/process_arrays", content);
        //
        //         var responseString = await response.Content.ReadAsStringAsync();
        //         Console.WriteLine($"Response: {responseString}");
        //     }
        // }
    }
}