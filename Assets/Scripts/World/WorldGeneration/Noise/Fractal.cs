using Newtonsoft.Json.Linq;

namespace World.WorldGeneration.Noise
{
    public static class Fractal
    {
        public static object FromJson(JObject jObject)
        {
            var fractal = new FastNoise($"Fractal {jObject["Argument"].ToString()}");
            fractal.Set("Gain", (float)(jObject["Gain"] ?? 0.5f));
            fractal.Set("Lacunarity", (float)(jObject["Lacunarity"] ?? 2.0f));
            fractal.Set("Octaves", (int)(jObject["Octaves"] ?? 3));
            fractal.Set("WeightedStrength", (float)(jObject["WeightedStrength"] ?? 0.5f));
            var generator = (NoiseGenerator)WorldDataParser.WorldDataParser.ParseData(jObject["Source"].ToObject<JObject>());
            fractal.Set("Source", generator.Noise);
            return fractal;
        }
    }
}