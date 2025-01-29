import numpy as np
import matplotlib.pyplot as plt
import json

# Define the size of the world and chunks
world_size = (100, 100)  # World dimensions in chunks
chunk_size = 1  # Size of each chunk

# Generate noise maps (dummy noise for now, replace with actual noise functions)
def generate_noise_map(size, seed=None):
    if seed:
        np.random.seed(seed)
    return np.random.rand(*size)

# Define biome decision tree
def determine_biome(erosion, continent, vegetation, temperature, humidity):
    if continent < 0.4:
        return "Ocean"
    elif erosion > 0.8:
        return "Mountain"
    elif vegetation > 0.6 and temperature > 0.5:
        if humidity > 0.6:
            return "Rainforest"
        else:
            return "Savanna"
    elif vegetation < 0.3:
        if temperature < 0.3:
            return "Tundra"
        else:
            return "Desert"
    else:
        return "Plains"

# Generate noise maps for parameters
erosion_map = generate_noise_map(world_size, seed=1)
continent_map = generate_noise_map(world_size, seed=2)
vegetation_map = generate_noise_map(world_size, seed=3)
temperature_map = generate_noise_map(world_size, seed=4)
humidity_map = generate_noise_map(world_size, seed=5)

# Assign biomes and collect parameters for JSON
biome_map = np.empty(world_size, dtype=object)
parameters = {}

for x in range(world_size[0]):
    for y in range(world_size[1]):
        erosion = erosion_map[x, y]
        continent = continent_map[x, y]
        vegetation = vegetation_map[x, y]
        temperature = temperature_map[x, y]
        humidity = humidity_map[x, y]

        biome = determine_biome(erosion, continent, vegetation, temperature, humidity)
        biome_map[x, y] = biome

        # Store parameters in JSON-compatible format
        parameters[(x, y)] = {
            "erosion": erosion,
            "continent": continent,
            "vegetation": vegetation,
            "temperature": temperature,
            "humidity": humidity,
            "biome": biome
        }

# Save parameters to JSON file
with open("biome_parameters.json", "w") as f:
    json.dump(parameters, f, indent=4)

# Visualize the biome map
biome_colors = {
    "Ocean": "blue",
    "Mountain": "gray",
    "Rainforest": "green",
    "Savanna": "yellow",
    "Tundra": "white",
    "Desert": "orange",
    "Plains": "lightgreen"
}

color_map = np.zeros((*world_size, 3))

for x in range(world_size[0]):
    for y in range(world_size[1]):
        biome = biome_map[x, y]
        color_map[x, y] = plt.colors.to_rgb(biome_colors[biome])

plt.imshow(color_map)
plt.title("Biome Map")
plt.axis("off")
plt.show()
