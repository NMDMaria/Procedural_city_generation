
# Procedural City Generation


The procedural city generation project by Chiriac Andrei-Octavian, Fălcescu Alexandru-Antonio, Negruț Maria-Daniela, and Plăcintescu Ștefan is designed for the "Programming simulation applications" class. It utilizes a Perlin noise generator, A*, and other procedural generation techniques to allow users to generate random cities and explore them.

![Image](https://i.gyazo.com/bfe72eb1ede24310a8820ada5975362f.png)
![Image](https://i.gyazo.com/baff8c3fb0ac36337e8fa1c45c473a78.png)
![Image](https://i.gyazo.com/6370ca46d5204e5acc9a61c760139604.png)
![Image](https://i.gyazo.com/7e2ebc5290bd691ac8a380ebe7d8e2d6.png)
![Image](https://i.gyazo.com/61ee52b59ab0b93cb15932430b8cbe3f.png)
![Image](https://i.gyazo.com/6ae9cbe4af5a1496e3a4c729ceff954c.png)

# Implementation

The process of generating the city is based on building the road system by placing entrances in the margins of the city and connecting them.

## Steps for creating the city:

1. **Create a matrix based on the user's input:** The implementation starts by creating a matrix that serves as the foundation for the city. The size of the matrix is determined by the user's input.

2. **Generate "obstacles" using Perlin Noise:** The Perlin Noise generator (implemented in PerlinGenerator.cs) is utilized to create "obstacles" within the matrix, which represent green sectors or parks. This script generates Perlin noise by iterating over the pixels of a texture and calculating the noise value using the Mathf.PerlinNoise() function. It assigns colors to the pixels based on the noise values and stores the noise pattern in a char matrix.

   **How it works:**
   - Iterate over the matrix and compute the color of each pixel using the PerlinNoise function.
   - Calculate the pixel color relative to the randomly generated offset (ensuring each generation produces a different segment of the noise map) and the noise scale.
   - Determine the value in the matrix - if the grayscale value surpasses a certain threshold, assign it as a park; otherwise, leave it as empty space.
   - The threshold value greatly influences the generation of the parks. Increasing it results in bigger and smoother parks, while decreasing it leads to more rigid shapes.
<div align="center">
  <img src="https://i.gyazo.com/7d265bafe3647b83329cb58e95eb07ff.png">
</div>

3. **Randomly generate entrances:** Entrances are randomly generated on the edges of the matrix, representing access points to the city.

4. **Create roads using the A-star algorithm:** The A* algorithm, adapted for matrix traversal with Manhattan distance, is employed to create paths between entrances on opposite sides of the map.

   **How it works:**
   - Compute the neighbors of the starting point and choose the one that is valid and has the best heuristic.
   - Ensure that each point is not visited multiple times.
   - Find the shortest path and compute it using the A* algorithm.
   - Assign the corresponding road markers in the city matrix.

5. **Remove isolated entrances (optional):** Optionally, isolated entrances that do not connect with any road can be removed from the city.

6. **Generate roads and buildings:** The actual roads and buildings are generated by instantiating prefabs based on the computed matrix.

   **How it works:**
   - Load the prefabs for the buildings, which have scripts indicating their size characteristics.
   - Traverse the matrix. When a road value is encountered, instantiate the road prefab at the corresponding address on the city's "plane." The same applies to grass.
   - If an empty space is encountered, generate a random building prefab. If placing the building would exceed the city's bounds or overlap with an obstacle or road, continue generating. If the building can be placed, assign the corresponding value in the matrix and instantiate the prefab at the center of the corresponding matrix tiles.

7. **Explore the city:** The city can be viewed with a day/night cycle, or the user can disable it. They can observe the city from an overhead perspective or press 'C' to freely roam through the city.

8. **Regenerate the city:** The user can regenerate the city by pressing the spacebar.

This implementation provides a structured approach to generate a city, complete with road systems, parks, and buildings. The use of Perlin Noise and the A* algorithm adds randomness and efficient pathfinding to create a diverse and functional urban environment.


## Authors

- [@Octi21](https://github.com/Octi21)
- [@Rodioo](https://github.com/Rodioo)
- [@NMDMaria](https://github.com/NMDMaria)
- [@WildCola](https://github.com/WildCola)

<div align="center">
  <img src="https://i.imgur.com/wMzXnvw.png" alt="Authors" width="15%" height="15%">
</div>







