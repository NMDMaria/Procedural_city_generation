using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGenerator : MonoBehaviour
{
    public static BlockGenerator instance = null;

    public int sizeX = 5;
    public int sizeY = 5;
    public bool roadMargin = true;
    public GameObject roadPrefab;
    public GameObject replacerPrefab;
    public Vector3 startPoint = Vector3.zero;
    

    public float roadOffset = 3f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Generate()
    {
        if (roadMargin)
        {
            // make a margin out of road prefab 
            for (int i = 0; i < sizeX; i++)
            {
                Instantiate(roadPrefab, new Vector3(i + roadOffset, 0, 0), Quaternion.identity);
                Instantiate(roadPrefab, new Vector3(i + roadOffset, 0, sizeY), Quaternion.identity);
            }
            for (int i = 0; i < sizeY; i++)
            {
                Instantiate(roadPrefab, new Vector3(0, 0, i + roadOffset), Quaternion.identity);
                Instantiate(roadPrefab, new Vector3(sizeX, 0, i + roadOffset), Quaternion.identity);
            }
        }


    }
}
