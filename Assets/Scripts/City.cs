using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour
{
    public int citySizeX = 16;
    public int citySizeY = 16;
    public int[,] city;
    public float procRoads = 0.55f;
    public float minProcRoads = 0.3f;

    public int nrEntrances = 3;

    private List<Vector2Int> margin;

    int nrRoads;

    public class Road {
        public Vector2Int coords;
        public bool isEntrance;
        public bool isIntersection;

        public override string ToString()
        {
            return coords.ToString();
        }
    }

    public class RoadCluster {
        public Vector2Int direction;
        public int roadsTillTurn;
        public List<Road> roads = new List<Road>();
        public bool isFinished = false;

        public override string ToString()
        {
            string output = "";
            output += direction;
            for (int j = 0; j < roads.Count; j++)
            {
               output += roads[j];
            }

            return output;
        }
    }

    private List<RoadCluster> openClusters;
    private List<RoadCluster> finishedClusters;

    // Start is called before the first frame update
    void Start()
    {
        margin = new List<Vector2Int>();
        openClusters = new List<RoadCluster>();
        finishedClusters = new List<RoadCluster>();

        city = new int[citySizeX, citySizeY];
        nrRoads = 0;

        for (int i = 0; i < citySizeX; i++)
        {
            for (int j = 0; j < citySizeY; j++)
            {
                if (i == 0 || j == 0 || i == citySizeX - 1 || j == citySizeY - 1)
                {
                    margin.Add(new Vector2Int(i, j));
                }
            }
        }

        // Define the entrance roads in the city by random.
        for (int i = 0; i < nrEntrances; i++)
        {
            Vector2Int entrance = margin[Random.Range(0, margin.Count - 1)];

            while (!shouldBeEntrance(entrance))
            {
                entrance = margin[Random.Range(0, margin.Count - 1)];
            }

            //   Got a point that could be an entrance, mark it as road
            city[entrance.x, entrance.y] = 1;
            nrRoads += 1;

            Road r = new Road();
            r.coords = entrance;
            r.isEntrance = true;
            r.isIntersection = false;

            RoadCluster rc = new RoadCluster();
            rc.roads.Add(r);

            Vector2Int dir = new Vector2Int(0, 0);

            
            if (entrance.x == 0)
            {
               dir = Vector2Int.right;
            } else if (entrance.x == citySizeX - 1)
            {
                dir = Vector2Int.left;
            } else if (entrance.y == 0)
            {
                dir = Vector2Int.up;
            } else {
                dir = Vector2Int.down;
            }

            rc.direction = dir;
            rc.roadsTillTurn = 1;

            openClusters.Add(rc);
        }

        int roadClustersFinished = 0;

        while (roadClustersFinished != 3) // make sure all road clusters reached an end
        {
            // Pick a random cluster to add another road
            int indexCluster = Random.Range(0, openClusters.Count - 1);

            bool makesTurn = Random.value > 0.6f;

            if (!makesTurn) 
            {
                Vector2Int nextTile = openClusters[indexCluster].roads[openClusters[indexCluster].roads.Count - 1].coords + openClusters[indexCluster].direction;
                int connectIdx = willConnect(nextTile);
                
                if (connectIdx != 0) 
                {
                    // connect 2 roads => intersection
                    Road r = new Road();
                    r.coords = nextTile;
                    r.isEntrance = false;
                    r.isIntersection = true;

                    openClusters[indexCluster].roads.Add(r);
                    openClusters[connectIdx].roads.InsertRange(0, openClusters[indexCluster].roads);
                    openClusters[indexCluster].isFinished = true;
                    
                    finishedClusters.Add(openClusters[indexCluster]);
                    openClusters.RemoveAt(indexCluster);
                } else if (validRoadPlacement(nextTile))
                {
                    Road r = new Road();
                    r.coords = nextTile;
                    if (nextTile.x == 0 || nextTile.x == citySizeX - 1 || nextTile.y == 0 || nextTile.y == citySizeY - 1)
                    {
                        // reached margin, end
                        r.isEntrance = true;
                        r.isIntersection = false;
                        openClusters[indexCluster].roads.Add(r);
                        openClusters[indexCluster].isFinished = true;
                        
                        finishedClusters.Add(openClusters[indexCluster]);
                        openClusters.RemoveAt(indexCluster);
                    } else {
                        r.isEntrance = false;
                        r.isIntersection = false;
                        openClusters[indexCluster].roads.Add(r);
                    }
                } 
            } else {
                // check what direction is possible to turn 

            }

            roadClustersFinished += 1;
        }


        string output = "";
        for (int i = 0; i < citySizeX; i++)
        {
            for (int j = 0; j < citySizeY; j++)
            {
                output += city[i, j] + " ";
            }
            output += "\n";
        }

        Debug.Log(output);

        for (int i = 0; i < openClusters.Count; i++)
        {

            Debug.Log(openClusters[i]);

        }
    }
    

    bool shouldBeEntrance(Vector2Int p)
    {
        int x = p.x;
        int y = p.y;

        if (city[p.x, p.y] == 1)
            return false;
        
        if ((x == 0 && y == citySizeY - 1) || (x == citySizeX - 1 && y == citySizeY - 1) 
        || (x == citySizeX - 1 && y == 0) || (x == 0 && y == 0) )
        {
            return false;
        }

        if (x + 1 >= 0 && x + 1 < citySizeX && city[x + 1, y] == 1)
            return false;
        
        if (x - 1 >= 0 && x - 1 < citySizeX && city[x - 1, y] == 1)
            return false;
                
        if (y - 1 >= 0 && y - 1 < citySizeY && city[x, y - 1] == 1)
            return false;

        if (y + 1 >= 0 && y + 1 < citySizeY && city[x, y + 1] == 1)
            return false;

        return true;
    }

    int willConnect(Vector2Int p, bool open=true)
    {
        // connect the 2 clusters if possible
        // return index

        return 0;
        if (open)
        {
            for (int i = 0; i < openClusters.Count; i++)
            {
                
            }
        }
    }


    bool validRoadPlacement(Vector2Int p)
    {
        int i = p.x;
        int j = p.y;

        if (i < 0 || j < 0 || i >= citySizeX || j >= citySizeY || city[i, j] != 0)
        {
            return false;
        }

        if ((i == 0 && j == citySizeY - 1) || (i == citySizeX - 1 && j == citySizeY - 1) 
        || (i == citySizeX - 1 && j == 0) || (i == 0 && j == 0) )
        {
            return false;
        }

        if (i - 1 > 0 && j - 1 > 0)
        {
            if (city[i - 1, j] != 0 && city[i - 1, j - 1] != 0 && city[i, j - 1] != 0)
            {
                // 1 1 
                // 1 () - cant also be road
                return false;
            }
        }

        if (i - 1 > 0 && j + 1 < citySizeY)
        {
            if (city[i - 1, j] != 0 && city[i - 1, j + 1] != 0 && city[i, j + 1] != 0)
            {
                // 1 1 
                // () 1 - cant also be road
                return false;
            }
        }

        if (i + 1 < citySizeX && j + 1 < citySizeY)
        {
            if (city[i + 1, j] != 0 && city[i + 1, j + 1] != 0 && city[i, j + 1] != 0)
            {
                // () 1 
                // 1 1 - cant also be road
                return false;
            }
        }

        if (i + 1 < citySizeX && j - 1 > 0)
        {
            if (city[i + 1, j] != 0 && city[i + 1, j - 1] != 0 && city[i, j - 1] != 0)
            {
                // 1 () 
                // 1 1 - cant also be road
                return false;
            }
        }

        return true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
