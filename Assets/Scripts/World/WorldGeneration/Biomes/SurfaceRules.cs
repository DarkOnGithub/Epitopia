using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using World.Blocks;
using Random = System.Random;

namespace World.WorldGeneration.Biomes
{
    
    public struct SurfaceRulesData
    {
        public string Type { get; set; }
        public int? Height { get; set; }
    }
    
    
    public class SurfaceRules
    {
        private Dictionary<int, Func<IBlockState>> _layers = new Dictionary<int, Func<IBlockState>>();
        private Func<IBlockState> _defaultRule;
        public int Depth = 0;
        
        public SurfaceRules(SurfaceRulesData[] data, JArray jArray)
        {
            
            var i = 0;
            foreach (var rule in data)
                ParseRule(rule, jArray[i++].ToObject<JObject>());
        }

        public void ParseRule(SurfaceRulesData rule, JObject jsonJObject)
        {
            switch (rule.Type)
            {
                case "Block":
                    Debug.Log(jsonJObject);
                    var block = BlockRegistry.GetBlock(jsonJObject["Argument"].Value<string>());
                    AddLayer(rule.Height, () => block.GetDefaultState());
                    break;
                case "Random":
                    var blockNames = jsonJObject["Argument"].Value<JArray>();
                    var blocks = new IBlock[blockNames.Count];
                    for (var i = 0; i < blockNames.Count; i++)
                        blocks[i] = BlockRegistry.GetBlock(blockNames[i].Value<string>());
                    var random = new Random();
                    AddLayer(rule.Height, () => blocks[random.Next(blocks.Length)].GetDefaultState());
                    break;
                    
            }
        }
        
        public void AddLayer(int? layerHeight, Func<IBlockState> rule)
        {
            if(layerHeight == null)
                _defaultRule = rule;
            else
            {
                Depth += layerHeight.Value;
                _layers[Depth] = rule;
            }
        }
        
        public Func<IBlockState> GetRule(int height)
        {
            foreach (var layer in _layers)
            {
                if (height <= layer.Key)
                {
                    return layer.Value;
                }
            }
            return _defaultRule;
        }
    }
}