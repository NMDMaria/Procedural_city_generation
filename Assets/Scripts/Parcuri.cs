using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parcuri : MonoBehaviour
{
    public int[,] matrix;

    public PerlinGenerator perlinGenerator;


    // Start is called before the first frame update
    void Start()
    {
        // generate random points => generate obstacle area
        // generate random points on the margin of the matrix
        // path find between oposing points => roads
        //matrix = Generate();
        //Debug.Log(matrix);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            matrix = Generate(); 
            int n = matrix.GetLength(0);
                            int m = matrix.GetLength(1);

            Debug.Log(matrix.GetLength(0));
            Debug.Log(matrix.GetLength(1));

            string output = "";

            for(int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    output += matrix[i, j] + " ";
                }
                output += "\n";
            }

            Debug.Log(output);


        }
    }

    private int[,] Generate()
    {
        return perlinGenerator.Generate();
        //roadGenerator.Generate();
        //gridSpawner.Generate();
    }
}
