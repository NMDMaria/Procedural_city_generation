using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerlinGenerator : MonoBehaviour
{
    public static PerlinGenerator instance = null;
    public int textureX;
    public int textureY;

    public Texture2D perlinTexture;
    public float noiseScale = 1f; // higher values result in larger and smoother patterns

    public Vector2 perlinOffset;

    public int perlinGridStepSizeX = 4;
    public int perlinGridStepSizeY = 4;
    public bool visualizeGrid = false;
    public GameObject visualizationCube;
    public float visualizationHeightScale = 5f;
    public RawImage visualizationUI;


    // Start is called before the first frame update
    void Start()
    {
        
    }

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

    // Update is called once per frame
    void Update()
    {
        
    }

    public int[,] Generate()
    {
       return  GenerateNoise();

        // 
    }

    private int[,] GenerateNoise()
    {
        perlinTexture = new Texture2D(textureX, textureY);
        perlinOffset = new Vector2(Random.Range(0, 99999), Random.Range(0, 99999));

        float threshold = 0.2f; // Set your desired threshold value here

        for (int i = 0; i < textureX; i++)
        {
            for (int j = 0; j < textureY; j++)
            {
                Color pixelColor = ColorPixel(i, j);
                if (pixelColor.grayscale < threshold)
                    perlinTexture.SetPixel(i, j, Color.white); // Use white color for values less than the threshold
                else
                    perlinTexture.SetPixel(i, j, Color.black); // Use black color for values greater than or equal to the threshold
            }
        }

        perlinTexture.Apply();
        visualizationUI.texture = perlinTexture;

        // Create the matrix
        int[,] matrix = new int[textureX, textureY];
        for (int i = 0; i < textureX; i++)
        {
            for (int j = 0; j < textureY; j++)
            {
                Color pixelColor = perlinTexture.GetPixel(i, j);
                matrix[i, j] = pixelColor == Color.white ? 1 : 0;
            }
        }

        string output = "";
        for (int i = 0; i < textureX; i++)
        {
            for (int j = 0; j < textureY; j++)
            {
                output += matrix[i, j] + " ";
            }
            output += "\n";
        }
        //Debug.Log(output);

        return matrix;

        // Now you have the matrix where a colored pixel less than the threshold means 1 and else 0
        // You can use the 'matrix' variable in your desired way
    }

    Color ColorPixel(int i, int j)
    {
        float xcoord = (float) i / textureX * noiseScale + perlinOffset.x;
        float ycoord = (float) j / textureY * noiseScale + perlinOffset.y;

        float val = Mathf.PerlinNoise(xcoord, ycoord);
        return new Color(val, val, val);
    }

}
