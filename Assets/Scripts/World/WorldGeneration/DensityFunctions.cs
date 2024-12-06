using Newtonsoft.Json.Linq;
using World.WorldGeneration.WorldDataParser;

namespace World.WorldGeneration
{
    public static class DensityFunctions
    {
        internal static FastNoise Add(JObject jsonObject)
        {
            var add = new FastNoise("Add");
            add.Set("LHS", (FastNoise)WorldDataParser.WorldDataParser.ParseData(jsonObject["Item1"].ToObject<JObject>()));
            add.Set("LHS", (FastNoise)WorldDataParser.WorldDataParser.ParseData(jsonObject["Item2"].ToObject<JObject>()));
            return add;
        }

        internal static FastNoise PositionOutput(JObject jObject)
        {
            var positionOutput = new FastNoise("PositionOutput");
            positionOutput.Set("MultiplierX", jObject.Get<float>("X"));
            positionOutput.Set("MultiplierY", jObject.Get<float>("Y"));
            positionOutput.Set("Offset X", jObject.Get<float>("X Offset"));
            positionOutput.Set("Offset Y", jObject.Get<float>("Y Offset"));
            return positionOutput;
        }
    }
}