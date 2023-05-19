using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    public static RoadGenerator instance = null;
    public float threshold = 0.5f; // the threshold value for the Perlin noise map
    public GameObject roadPrefab; // the prefab for the road object
    private bool[,] binaryMap; // the binary map derived from the Perlin noise map

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void Generate()
    {
        Color[] pixels = PerlinGenerator.instance.perlinTexture.GetPixels();
        int width =  PerlinGenerator.instance.textureX;
        int height =  PerlinGenerator.instance.textureY;
        
        binaryMap = new bool[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Color pixelColor = pixels[i * width + j];
                binaryMap[i, j] = pixelColor.grayscale < threshold;
            }
        }

        // Generate connected graph
        List<Vector2Int> visited = new List<Vector2Int>();
        List<Vector2Int> connectedNodes = new List<Vector2Int>();
        Vector2Int startNode = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
        connectedNodes.Add(startNode);
        visited.Add(startNode);

        while (connectedNodes.Count > 0)
        {
            Vector2Int currentNode = connectedNodes[0];
            connectedNodes.RemoveAt(0);

            // Add neighbors to connectedNodes list
            if (currentNode.x > 0 && binaryMap[currentNode.x - 1, currentNode.y] && !visited.Contains(new Vector2Int(currentNode.x - 1, currentNode.y)))
            {
                connectedNodes.Add(new Vector2Int(currentNode.x - 1, currentNode.y));
                visited.Add(new Vector2Int(currentNode.x - 1, currentNode.y));
            }
            if (currentNode.x < width - 1 && binaryMap[currentNode.x + 1, currentNode.y] && !visited.Contains(new Vector2Int(currentNode.x + 1, currentNode.y)))
            {
                connectedNodes.Add(new Vector2Int(currentNode.x + 1, currentNode.y));
                visited.Add(new Vector2Int(currentNode.x + 1, currentNode.y));
            }
            if (currentNode.y > 0 && binaryMap[currentNode.x, currentNode.y - 1] && !visited.Contains(new Vector2Int(currentNode.x, currentNode.y - 1)))
            {
                connectedNodes.Add(new Vector2Int(currentNode.x, currentNode.y - 1));
                visited.Add(new Vector2Int(currentNode.x, currentNode.y - 1));
            }
            if (currentNode.y < height - 1 && binaryMap[currentNode.x, currentNode.y + 1] && !visited.Contains(new Vector2Int(currentNode.x, currentNode.y + 1)))
            {
                connectedNodes.Add(new Vector2Int(currentNode.x, currentNode.y + 1));
                visited.Add(new Vector2Int(currentNode.x, currentNode.y + 1));
            }
        }


        // Spawn roadPrefab on true values
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (visited.Contains(new Vector2Int(i, j)))
                {
                    Vector3 position = new Vector3(i + 3, 0, j + 3);
                    Instantiate(roadPrefab, position, Quaternion.identity);
                }
            }
        }

    }
}
