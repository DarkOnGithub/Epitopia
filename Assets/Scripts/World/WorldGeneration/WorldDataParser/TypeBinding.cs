using System;
using System.Collections.Generic;
using Core;
using Newtonsoft.Json.Linq;
using World.WorldGeneration.Noise;

namespace World.WorldGeneration.WorldDataParser
{
    public static class TypeBinding
    {
        
        private static BetterLogger _logger = new(typeof(TypeBinding));
        public static Dictionary<string, Func<JObject, object>> TypeBindings = new()
        {

        };

		public static object CallBinding(string type, JObject jObject)
        {
            if (TypeBindings.TryGetValue(type.ToLower().Replace(" ",""), out var binding))
                return binding(jObject);
            
            _logger.LogError($"No binding found for type {type} -> {jObject}");
            return null;
        }
    }
}