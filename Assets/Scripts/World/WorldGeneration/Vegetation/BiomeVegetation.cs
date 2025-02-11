using System;
using System.Collections.Concurrent;
using UnityEngine;
using World.Blocks;
using World.Chunks;
using Random = System.Random;

namespace World.WorldGeneration.Vegetation
{
    public class BiomeVegetation
    {
        ConcurrentDictionary<(float start, float end, float prob), IVegetationComponent> _vegetationComponents = new();
        
        public BiomeVegetation(VegetationComponentData[] vegetationComponents)
        {
            foreach (var vegetationComponent in vegetationComponents)
            {
                IVegetationComponent component = null;
                switch (vegetationComponent.Type)
                {
                    case "Flower":
                        component = new Flower(BlockRegistry.GetBlock(vegetationComponent.Block));
                        break;
                    case "Tree":
                        component = new Tree((vegetationComponent.Block));
                        break;
                }
                if(component == null)
                    return;
                _vegetationComponents.TryAdd((vegetationComponent.Range[0], vegetationComponent.Range[1], vegetationComponent.Probability), component);
            }
        }
        
        public void GenerateVegetation(Chunk chunk, float noise, Vector2Int localPosition, Vector2Int position)
        {
 
            foreach (var (range, component) in _vegetationComponents)
            {
                if (noise >= range.start && noise <= range.end &&
                    component.CanGenerateAt(chunk, localPosition, position, chunk.WorldIn))
                {
                    var random = new Random(Seed.SeedValue + localPosition.x + localPosition.y);
                    if (random.NextDouble() <= range.prob)
                        component.Generate(chunk, localPosition, position, chunk.WorldIn);
                }
            }
        
     
      
        }
    }
}