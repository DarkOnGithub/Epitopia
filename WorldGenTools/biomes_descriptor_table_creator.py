import matplotlib.pyplot as plt
import numpy as np

def find_biome(erosion, continentalness, temperature, humidity, biomes):
    # ... (same find_biome function as before)
    best_biome = None
    min_distance = float('inf')

    for biome in biomes:
        is_match = True
        for param, value in biome['ranges'].items():
            noise_value = locals()[param.lower()]
            if not (value['min'] <= noise_value <= value['max']):
                is_match = False
                break

        if is_match:
            return biome['name'], biome.get('color')

        distance = 0
        for param, value in biome['ranges'].items():
            noise_value = locals()[param.lower()]
            if noise_value < value['min']:
                distance += value['min'] - noise_value
            elif noise_value > value['max']:
                distance += noise_value - value['max']

        if distance < min_distance:
            min_distance = distance
            best_biome = biome

    return best_biome['name'], best_biome.get('color') if best_biome else (None, None)

# Define your biomes with colors
biomes = [
    {
        'name': 'Forest',
        'ranges': {
            'Erosion': {'min': 0.05, 'max': 0.10},
            'Continentalness': {'min': 0.12, 'max': 0.18},
            'Temperature': {'min': 0.15, 'max': 0.20},
            'Humidity': {'min': 0.08, 'max': 1.0},
        },
        'color': 'green'
    },
    {
        'name': 'Desert',
        'ranges': {
            'Erosion': {'min': 0.01, 'max': 0.03},
            'Continentalness': {'min': 0.01, 'max': 0.05},
            'Temperature': {'min': 0.25, 'max': 0.29},
            'Humidity': {'min': 0.01, 'max': 0.3},
        },
        'color': 'yellow'
    },
    {
        'name': 'Grassland',
        'ranges': {
            'Erosion': {'min': 0.03, 'max': 0.07},
            'Continentalness': {'min': 0.08, 'max': 0.14},
            'Temperature': {'min': 0.10, 'max': 0.16},
            'Humidity': {'min': 0.3, 'max': 0.7},
        },
        'color': 'lightgreen'
    },
    {
        'name': 'Tundra',
        'ranges': {
            'Erosion': {'min': 0.15, 'max': 0.25},
            'Continentalness': {'min': 0.20, 'max': 0.29},
            'Temperature': {'min': 0.01, 'max': 0.08},
            'Humidity': {'min': 0.5, 'max': 0.9},
        },
        'color': 'skyblue'
    },
    # Add more biomes with their respective colors
]

noise_parameters = ['Erosion', 'Continentalness', 'Temperature', 'Humidity']

# Define the range for the varying parameter
param_range = np.linspace(0.01, 1, 50)  # Adjust resolution as needed

# Fix the other parameters to a constant value (you can experiment with these)
fixed_values = {
    'Erosion': 0.05,
    'Continentalness': 0.15,
    'Temperature': 0.18,
    'Humidity': 0.5
}

fig, axes = plt.subplots(nrows=2, ncols=2, figsize=(12, 8))
axes = axes.flatten()  # Flatten the 2x2 array of axes for easier indexing

for i, varying_param_name in enumerate(noise_parameters):
    ax = axes[i]
    biome_names = []
    biome_colors = []

    for param_value in param_range:
        current_params = fixed_values.copy()
        current_params[varying_param_name] = param_value
        biome_name, biome_color = find_biome(
            erosion=current_params['Erosion'],
            continentalness=current_params['Continentalness'],
            temperature=current_params['Temperature'],
            humidity=current_params['Humidity'],
            biomes=biomes
        )
        biome_names.append(biome_name if biome_name else "No Biome")
        biome_colors.append(biome_color if biome_color else 'lightgrey')

    # Create a mapping of biome names to colors for the bars
    unique_biomes = sorted(list(set(biome_names)))
    color_map = {biome: color for biome, color in zip(biome_names, biome_colors)}
    bar_colors = [color_map[biome] for biome in biome_names]

    ax.bar(param_range, [1] * len(param_range), color=bar_colors, width=(param_range[1] - param_range[0]))

    ax.set_xlabel(varying_param_name)
    ax.set_ylabel("Biome")  # Just indicating the presence of a biome
    ax.set_title(f'Varying {varying_param_name}')
    ax.set_yticks([]) # Remove y-axis ticks

    # Create a legend (only for the first subplot to avoid redundancy)
    if i == 0:
        import matplotlib.patches as mpatches
        patches = [mpatches.Patch(color=color_map.get(biome, 'lightgrey'), label=biome) for biome in unique_biomes]
        ax.legend(handles=patches, bbox_to_anchor=(1.05, 1), loc='upper left')

plt.suptitle("Biome Distribution with Varying Parameters", fontsize=16)
plt.tight_layout(rect=[0, 0, 0.9, 1.0]) # Adjust layout to make space for the legend
plt.show()