using System;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace World.WorldGeneration.DensityFunctions
{
    public static class DensityFunctionHelper
    {
        public static object ParseStruct(JObject jObject, Type type)
        {
            var noiseObject = new FastNoise((type.Name));
            foreach (var property in type.GetProperties())
            {
                try
                {
                    var propertyName = property.Name.Replace("__", "");
                    if(jObject[propertyName] == null)
                        continue;
                    var value = jObject[propertyName].ToObject(property.PropertyType);
                    switch (value)
                    {
                        case int intValue:
                            noiseObject.Set(propertyName, intValue);
                            break;
                        case float floatValue:
                            noiseObject.Set(propertyName, floatValue);
                            break;
                        case string stringValue:
                            noiseObject.Set(propertyName, stringValue);
                            break;
                        case FastNoise fastNoiseValue:
                            noiseObject.Set(propertyName, fastNoiseValue);
                            break;
                        default:
                            throw new InvalidOperationException($"Unsupported property type: {property.PropertyType}");
                    }
                }catch (Exception e)
                {
                    Debug.Log($"Error when parsing {type.Name} with {property.Name} : {e.Message}");
                }
            }

            return noiseObject;
        }
        
        public static string CleanString(string input)
        {
            return input.ToLower().Replace(" ", "");
        }
    }
}