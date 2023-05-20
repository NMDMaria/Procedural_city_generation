using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parcuri : MonoBehaviour
{
    public char[,] matrix;
    public int x;
    public int y;

    public int nrEntrances = 3;

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
            x = matrix.GetLength(0);
            y = matrix.GetLength(1);

            List<Vector2Int> topE = new List<Vector2Int>();
            List<Vector2Int> botE = new List<Vector2Int>();
            List<Vector2Int> rightE = new List<Vector2Int>();
            List<Vector2Int> leftE = new List<Vector2Int>();

            List<Vector2Int> margin = new List<Vector2Int>();

            // Start with left side
            for (int i = 0; i < x; i++)
            {
                margin.Add(new Vector2Int(i, 0));               
            }

            topE = AddEntrance(margin);

            margin.Clear();

            // Start with right side
            for (int i = 0; i < x; i++)
            {
                margin.Add(new Vector2Int(i, y - 1));               
            }

            rightE = AddEntrance(margin);

            margin.Clear();

            // Start with top side
            for (int j = 0; j < y; j++)
            {
                margin.Add(new Vector2Int(0, j));               
            }

            topE = AddEntrance(margin);

            margin.Clear();

            // Start with end side
            for (int j = 0; j < y; j++)
            {
                margin.Add(new Vector2Int(x - 1, j));               
            }

            botE = AddEntrance(margin);


            // for (int i = 0; i < topE.Count; i++)
            // {
            //     int connect_idx = Random.Range(0, botE.Count - 1);
            //     BFS.getPath(matrix, topE[i].x, topE[i].y, botE[connect_idx].x, botE[connect_idx].y);
            //     botE.RemoveAt(connect_idx);
            // }


            string output = "";

            for(int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    output += matrix[i, j] + " ";
                }
                output += "\n";
            }

            Debug.Log(output);


        }
    }


    private List<Vector2Int> AddEntrance(List<Vector2Int> margin)
    {
        List<Vector2Int> added_points = new List<Vector2Int>();
        // Define the entrance roads in the city by random.
        for (int n = 0; n < nrEntrances; n++)
        {
            Vector2Int entrance = margin[Random.Range(0, margin.Count - 1)];

            while (!shouldBeEntrance(entrance))
            {
                entrance = margin[Random.Range(0, margin.Count - 1)];
            }

            //   Got a point that could be an entrance, mark it as road
            matrix[entrance.x, entrance.y] = 'D';
            added_points.Add(new Vector2Int(entrance.x, entrance.y));
        }

        return added_points;
    }

    private bool shouldBeEntrance(Vector2Int p)
    {
        if (matrix[p.x, p.y] != '0')
            return false;
        
        if ((p.x == 0 && p.y == y - 1) || (p.x == x - 1 && p.y == y - 1) 
        || (p.x == x - 1 && p.y == 0) || (p.x == 0 && p.y == 0) )
        {
            return false;
        }

        if (p.x + 1 >= 0 && p.x + 1 < x && matrix[p.x + 1, p.y] == 'D')
            return false;
        
        if (p.x - 1 >= 0 && p.x - 1 < x && matrix[p.x - 1, p.y] == 'D')
            return false;
                
        if (p.y - 1 >= 0 && p.y - 1 < y && matrix[p.x, p.y - 1] == 'D')
            return false;

        if (p.y + 1 >= 0 && p.y + 1 < y && matrix[p.x, p.y + 1] == 'D')
            return false;

        return true;
    }


    private char[,] Generate()
    {
        return perlinGenerator.Generate();
    }
}
