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

    public void Generate()
    {
        GenerateNoise();

        // 
    }

    private void GenerateNoise()
    {
        perlinTexture = new Texture2D(textureX, textureY);
        perlinOffset = new Vector2(Random.Range(0, 99999), Random.Range(0, 99999));

        for (int i = 0; i < textureX; i++)
        {
            for (int j = 0; j < textureY; j++)
            {
                perlinTexture.SetPixel(i, j, ColorPixel(i, j));
            }
        }

        perlinTexture.Apply();
        visualizationUI.texture = perlinTexture;
    }

    Color ColorPixel(int i, int j)
    {
        float xcoord = (float) i / textureX * noiseScale + perlinOffset.x;
        float ycoord = (float) j / textureY * noiseScale + perlinOffset.y;

        float val = Mathf.PerlinNoise(xcoord, ycoord);
        return new Color(val, val, val);
    }

}
