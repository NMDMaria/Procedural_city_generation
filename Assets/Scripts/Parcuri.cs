using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
//using System.Diagnostics;
using System.Security.Principal;
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

    private void printArr(char[] arr)
    {
        string output = "";

        for(int i = 0; i < arr.Length; i++)
        {
            output += arr[i] + " ";
        }

        Debug.Log(output);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            matrix = Generate(); 
            x = matrix.GetLength(0);
            y = matrix.GetLength(1);
            int n = x;
            int m = y;

            List<Vector2Int> topE = new List<Vector2Int>();
            List<Vector2Int> botE = new List<Vector2Int>();
            List<Vector2Int> rightE = new List<Vector2Int>();
            List<Vector2Int> leftE = new List<Vector2Int>();

            char[] l1 = new char[m];
            char[] l2 = new char[m];
            char[] c1 = new char[n];
            char[] c2 = new char[n];

            for (int i = 0; i < n; i++)
            {
                c1[i] = matrix[i, 0];
                c2[i] = matrix[i, m - 1];
            }
            for (int i = 0; i < m; i++)
            {
                l1[i] = matrix[0, i];
                l2[i] = matrix[n-1, i];
            }
            Debug.Log(" l1:");

            //printArr(l1);


            // l1
            int replaceCount = UnityEngine.Random.Range(1, 4); // Random number between 1 and 3
            int replaced = 0;

            for(int i = 0; i < replaceCount; i++)
            //while(replaced < replaceCount)
            {
                int randomPosition = UnityEngine.Random.Range(1, m);
                if(l1[randomPosition] == '0')
                {
                    if(randomPosition>0 && l1[randomPosition - 1] != 'D' && randomPosition < m - 1 && l1[randomPosition + 1] != 'D')
                    {
                        topE.Add(new Vector2Int(0, randomPosition));
                        l1[randomPosition] = 'D';
                        replaced++;
                    }
                    
                }

                if (replaced >= replaceCount)
                    break;
            }


            //Debug.Log("Updated l1:");
            //printArr(l1);

            
            
            //l2
            replaceCount = UnityEngine.Random.Range(1, 4); // Random number between 1 and 3
            replaced = 0;

            for (int i = 0; i < replaceCount; i++)
            //while (replaced < replaceCount)

            {
                int randomPosition = UnityEngine.Random.Range(1, m);
                if (l2[randomPosition] == '0')
                {
                    if (randomPosition > 0 && l2[randomPosition - 1] != 'D' && randomPosition < m - 1 && l2[randomPosition + 1] != 'D')
                    {
                        botE.Add(new Vector2Int(m -1, randomPosition));
                        l2[randomPosition] = 'D';
                        replaced++;
                    }

                }

                if (replaced >= replaceCount)
                    break;
            }


            //Debug.Log("Updated l2:");
            //printArr(l2);


            //c1
            replaceCount = UnityEngine.Random.Range(1, 4); // Random number between 1 and 3
            replaced = 0;

            for (int i = 0; i < replaceCount; i++)
            //while (replaced < replaceCount)

            {
                int randomPosition = UnityEngine.Random.Range(1, n);
                if (c1[randomPosition] == '0')
                {
                    if (randomPosition > 0 && c1[randomPosition - 1] != 'D' && randomPosition < m - 1 && c1[randomPosition + 1] != 'D')
                    {
                        leftE.Add(new Vector2Int(randomPosition, 0));
                        c1[randomPosition] = 'D';
                        replaced++;
                    }

                }

                if (replaced >= replaceCount)
                    break;
            }


            //Debug.Log("Updated c1:");
            //printArr(c1);


            //c2
            replaceCount = UnityEngine.Random.Range(1, 4); // Random number between 1 and 3
            replaced = 0;

            for (int i = 0; i < replaceCount; i++)
            //while (replaced < replaceCount)

            {
                int randomPosition = UnityEngine.Random.Range(1, n);
                if (c2[randomPosition] == '0')
                {
                    if (randomPosition > 0 && c2[randomPosition - 1] != 'D' && randomPosition < m - 1 && c2[randomPosition + 1] != 'D')
                    {
                        rightE.Add(new Vector2Int(randomPosition, n-1));

                        c2[randomPosition] = 'D';
                        replaced++;
                    }

                }

                if (replaced >= replaceCount)
                    break;
            }


            //Debug.Log("Updated c2:");
            //printArr(c2);



            ////     inlocuire in matrice 


            for (int i = 0; i < n; i++)
            {
                matrix[i, 0] = c1[i];
                matrix[i, m - 1] = c2[i];
            }
            for (int i = 0; i < m; i++)
            {
                matrix[0, i] = l1[i];
                matrix[n - 1, i] = l2[i];
            }

            for (int i = 0; i < topE.Count; i++)
            {
                int idx = Random.Range(0, botE.Count - 1);
                if (botE.Count <= 0)
                    break;
                BFS.getPath(matrix, topE[i].x, topE[i].y, botE[idx].x, botE[idx].y);
                botE.RemoveAt(idx);
            }

            for (int i = 0; i < leftE.Count; i++)
            {
                int idx = Random.Range(0, rightE.Count - 1);
                if (rightE.Count <= 0)
                    break;
                BFS.getPath(matrix, leftE[i].x, leftE[i].y, rightE[idx].x, rightE[idx].y);
                rightE.RemoveAt(idx);
            }



            /*List<Vector2Int> margin = new List<Vector2Int>();

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

            botE = AddEntrance(margin);*/


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
/*int[] l1 = new int[m];
          int[] l2 = new int[m];
          int[] c1 = new int[n];
          int[] c2 = new int[n];

          for (int i = 0; i < n; i++)
          {
              c1[i] = matrix[i, 0];
              c2[i] = matrix[i, m - 1];
          }
          for (int i = 0; i < m; i++)
          {
              l1[i] = matrix[0, i];
              l2[i] = matrix[n-1, i];
          }
          Debug.Log(" l1:");

          printArr(l1);

          int replaceCount = UnityEngine.Random.Range(1, 4); // Random number between 1 and 3
          int replaced = 0;
          List<int> indices = new List<int>();

          for(int i = 0; i < replaceCount; i++)
          {
              int randomPosition = UnityEngine.Random.Range(1, m);
              if(l1[randomPosition] == 0)
              {
                  l1[randomPosition] = 1;
                   replaced ++;
              }

              if (replaced >= replaceCount)
                  break;
          }


          Debug.Log("Updated l1:");
          printArr(l1);

          */