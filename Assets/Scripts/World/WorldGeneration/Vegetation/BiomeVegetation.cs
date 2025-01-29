using System.Collections.Concurrent;
using UnityEngine;
using World.Blocks;
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
                }
                _vegetationComponents.TryAdd((vegetationComponent.Range[0], vegetationComponent.Range[1], vegetationComponent.Probability), component);
            }
        }
        
        public void GenerateVegetation(IBlockState[] chunkIn, float noise, Vector2Int localPosition)
        {
            foreach (var (range, component) in _vegetationComponents)
            {
                if (noise >= range.start && noise <= range.end && component.CanGenerateAt(chunkIn, localPosition))
                {
                    var random = new Random(Seed.SeedValue + localPosition.x + localPosition.y);
                    if (random.NextDouble() <= range.prob)
                        component.Generate(chunkIn, localPosition);
                }
            }
        }
    }
}