using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;


public class Parcuri : MonoBehaviour
{
    public char[,] matrix;
    public int x;
    public int y;

    public int nrEntrances = 3;

    public Toggle toggle;
    public bool isCycle;

    public PerlinGenerator perlinGenerator;

    // Start is called before the first frame update
    void Start()
    {
        // generate random points => generate obstacle area
        // generate random points on the margin of the matrix
        // path find between oposing points => roads
        //matrix = Generate();
        //Debug.Log(matrix);
        toggle.GetComponentInChildren<Text>().text = "Enable Day/Night Cycle";
        isCycle = toggle.isOn;
    }

    public void OnToggleValueChanged()
    {
        isCycle = toggle.isOn;
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

    private int maxEntries(int n)
    {
        int result = 2 * (int)Math.Floor(Math.Sqrt(n));
        return Mathf.Max(2, result);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            deleteDrum();

            matrix = Generate(); 
            x = matrix.GetLength(0);
            y = matrix.GetLength(1);
            int n = x;
            int m = y;
            int maxN = maxEntries(n);
            int maxM = maxEntries(m);



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
                c1[i] = matrix[i, 0];    // coloana din stanga
                c2[i] = matrix[i, m - 1];  // coloana din dreapta
            }
            for (int i = 0; i < m; i++)
            {
                l1[i] = matrix[0, i];     // linia de sus
                l2[i] = matrix[n-1, i];    // linia de jos 
            }
            


            // l1
            int replaceCount = UnityEngine.Random.Range(1, maxM); // Random number between 1 and 2
            int replaced = 0;

            //Debug.Log("Nr. of roads: ");
            //Debug.Log(replaceCount);
            
            while (replaced < replaceCount)
            {
                int randomPosition = UnityEngine.Random.Range(1, m-1);

                if(l1[randomPosition] == '0')
                {
                    if(randomPosition>0 && l1[randomPosition - 1] != 'D' && randomPosition < m - 1 && l1[randomPosition + 1] != 'D')
                    {
                        topE.Add(new Vector2Int(0, randomPosition));
                        l1[randomPosition] = 'D';
                        replaced++;
                    }
                    
                }
            }
            // Debug.Log(" l1:");

            // printArr(l1);

            //l2
            replaceCount = UnityEngine.Random.Range(1, maxM); // Random number between 1 and 2
             replaced = 0; 

            while (replaced < replaceCount)
            {
                int randomPosition = UnityEngine.Random.Range(1, m-1);
                if (l2[randomPosition] == '0')
                {
                    if (randomPosition > 0 && l2[randomPosition - 1] != 'D' && randomPosition < m - 1 && l2[randomPosition + 1] != 'D')
                    {
                        botE.Add(new Vector2Int(m -1, randomPosition));
                        l2[randomPosition] = 'D';
                        replaced++;
                    }
                }
            }
            //Debug.Log(" l2:");

            //printArr(l2);

            replaceCount = UnityEngine.Random.Range(1, maxN); // Random number between 1 and 2
             replaced = 0;

            while (replaced < replaceCount)
            {
                int randomPosition = UnityEngine.Random.Range(1, n-1);
                if (c1[randomPosition] == '0')
                {
                    if (randomPosition > 0 && c1[randomPosition - 1] != 'D' && randomPosition < m - 1 && c1[randomPosition + 1] != 'D')
                    {
                        leftE.Add(new Vector2Int(randomPosition, 0));
                        c1[randomPosition] = 'D';
                        replaced++;
                    }
                }
            }


             replaceCount = UnityEngine.Random.Range(1, maxN); // Random number between 1 and 2
             replaced = 0;

            while (replaced < replaceCount)
            {
                int randomPosition = UnityEngine.Random.Range(1, n-1);
                if (c2[randomPosition] == '0')
                {
                    if (randomPosition > 0 && c2[randomPosition - 1] != 'D' && randomPosition < m - 1 && c2[randomPosition + 1] != 'D')
                    {
                        rightE.Add(new Vector2Int(randomPosition, n-1));

                        c2[randomPosition] = 'D';
                        replaced++;
                    }

                }
            }
            //Debug.Log(" c1:");

            //printArr(c1);
            //Debug.Log(" c2:");

            //printArr(c2);


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
                int idx = UnityEngine.Random.Range(0, botE.Count - 1);
                if (botE.Count <= 0)
                    break;
                //BFS.getPath(matrix, topE[i].x, topE[i].y, botE[idx].x, botE[idx].y);
                BFS.getPathAStar(matrix, topE[i].x, topE[i].y, botE[idx].x, botE[idx].y);
                botE.RemoveAt(idx);
            }

            for (int i = 0; i < leftE.Count; i++)
            {
                int idx = UnityEngine.Random.Range(0, rightE.Count - 1);
                if (rightE.Count <= 0)
                    break;
                //BFS.getPath(matrix, leftE[i].x, leftE[i].y, rightE[idx].x, rightE[idx].y);
                BFS.getPathAStar(matrix, leftE[i].x, leftE[i].y, rightE[idx].x, rightE[idx].y);
                rightE.RemoveAt(idx);
            }

            removeIsolated();

            string output = "";

            for(int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    output += matrix[i, j] + " ";
                }
                output += "\n";
            }

            //Debug.Log(output);

            genDrum();
        }
    }


    private List<Vector2Int> AddEntrance(List<Vector2Int> margin)
    {
        List<Vector2Int> added_points = new List<Vector2Int>();
        // Define the entrance roads in the city by random.
        for (int n = 0; n < nrEntrances; n++)
        {
            Vector2Int entrance = margin[UnityEngine.Random.Range(0, margin.Count - 1)];

            while (!shouldBeEntrance(entrance))
            {
                entrance = margin[UnityEngine.Random.Range(0, margin.Count - 1)];
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

    private void removeIsolated()
    {
        for (int j = 1; j < y - 1; j++)
        {
            // 0 D 0 0 
            // 0 0
            if (matrix[0, j] == 'D')
            {
                if (matrix[0, j + 1] != 'D' && matrix[0, j - 1] != 'D' && matrix[1, j] != 'D')
                {
                    // isolated case
                    matrix[0, j] = '0';
                }
            }

            if (matrix[x - 1, j] == 'D')
            {
                if (matrix[x - 1, j + 1] != 'D' && matrix[x - 1, j - 1] != 'D' && matrix[x - 2, j] != 'D')
                {
                    // isolated case
                    matrix[x - 1, j] = '0';
                }
            }
        }

        for (int i = 1; i < x - 1; i++)
        {
            // 0 
            // D 0
            // 0
            if (matrix[i, 0] == 'D')
            {
                if (matrix[i + 1, 0] != 'D' && matrix[i - 1, 0] != 'D' && matrix[i, 1] != 'D')
                {
                    // isolated case
                    matrix[i, 0] = '0';
                }
            }

            if (matrix[i, y - 1] == 'D')
            {
                if (matrix[i + 1, y - 1] != 'D' && matrix[i - 1, y - 1] != 'D' && matrix[i, y - 2] != 'D')
                {
                    // isolated case
                    matrix[i, y - 1] = '0';
                }
            }        
        }
    }


    private char[,] Generate()
    {
        return perlinGenerator.Generate();
    }

    List<GameObject> roadInstances = new List<GameObject>();
    List<GameObject> grassInstances = new List<GameObject>();
    List<GameObject> buildInstances = new List<GameObject>();
    UnityEngine.Object[] buildings;
    GameObject[] buildings_prefabs;

    private void genDrum(){
        int tileSize = 3;
        GameObject road;
        GameObject grass;

        buildings = Resources.LoadAll("Ghe");
        buildings_prefabs = new GameObject[buildings.Length];
        for (int i = 0; i < buildings.Length; i++)
        {
            buildings_prefabs[i] = (GameObject)buildings[i];
            buildings_prefabs[i].transform.position = new Vector3(0, 0, 0);
            // buildings_prefabs[i].transform.localScale = new Vector3(1f, 1f, 1f);
        }

        road = Resources.Load<GameObject>("Roads/road_black");
        grass = Resources.Load<GameObject>("Roads/green");

        Debug.Log(x);

        for (int i=0; i<x; ++i){
            for (int j=0; j<y; ++j){
                if (matrix[i, j] == 'D'){
                    roadInstances.Add(Instantiate(road));
                    roadInstances[roadInstances.Count - 1].transform.position = new Vector3(3*i, 0, 3*j);
                }
                else if (matrix[i, j] == 'P')
                {
                    grassInstances.Add(Instantiate(grass));
                    grassInstances[grassInstances.Count - 1].transform.position = new Vector3(3 * i, -0.1f, 3 * j);
                }
                else if (matrix[i, j] == '0')
                {
                    bool found_building = false;
                    while (found_building == false)
                    {
                        found_building = true;
                        int index_b = UnityEngine.Random.Range(0, buildings_prefabs.Length);
                        //bool oreintation = Random.Range(0, 2) == 1;
                        int xb = buildings_prefabs[index_b].GetComponent<BuildingSize>().x;
                        int yb = buildings_prefabs[index_b].GetComponent<BuildingSize>().y;
                        float height = buildings_prefabs[index_b].GetComponent<BuildingSize>().h / 2;
                        //Debug.Log(xb+" "+yb);

                        if (xb+i > x || yb+j > y)
                            continue;

                        for (int k=0; k < xb; ++k)
                            for (int l=0; l < yb; ++l)
                                if (matrix[i+k, j+l] != '0')
                                    found_building = false;

                        if (found_building == false)
                            continue;

                        for (int k=0; k < xb; ++k)
                            for (int l=0; l < yb; ++l)
                                matrix[i+k, j+l] = 'B';

                        float bx = i+(xb/2);
                        float by = j+(yb/2);
                        if (xb > 1)
                            bx -= 0.5f;
                        if (yb > 1)
                            by -= 0.5f;

                        buildInstances.Add(Instantiate(buildings_prefabs[index_b]));
                        buildInstances[buildInstances.Count - 1].transform.position = new Vector3(3*bx, height, 3*by);

                    }

                }
            }
        }

    }

    private void deleteDrum() {
        for (int i = 0; i < roadInstances.Count; ++i)
            Destroy(roadInstances[i]);
        roadInstances.Clear();
        for (int i = 0; i < grassInstances.Count; ++i)
            Destroy(grassInstances[i]);
        grassInstances.Clear();
        for (int i = 0; i < buildInstances.Count; ++i)
            Destroy(buildInstances[i]);
        buildInstances.Clear();
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