import json
import os

# The JSON data (you can also load this from a file)
biomes_data = open("./BDT.json", "r").read()

def create_biome_files():
    # Create the directory path if it doesn't exist
    output_dir = "Assets/Resources/Config/WorldGeneration/Biomes"
    os.makedirs(output_dir, exist_ok=True)
    
    # Parse the JSON data
    biomes = json.loads(biomes_data)
    
    # Create a file for each biome
    for biome in biomes:
        # Create a filename from the biome name
        filename = biome["Name"].replace(" ", "").replace("-", "") + ".json"
        filepath = os.path.join(output_dir, filename)
        
        # Write the formatted JSON to file
        with open(filepath, 'w') as f:
            json.dump(biome, f, indent=4)
        
        print(f"Created biome file: {filename}")

if __name__ == "__main__":
    create_biome_files()
