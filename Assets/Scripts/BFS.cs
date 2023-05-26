using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BFS: MonoBehaviour
{
    private static int[] dx = { -1, 0, 1, 0 }; // Offsets for moving in four directions: up, right, down, left
    private static int[] dy = { 0, 1, 0, -1 };

    private static bool checkRoadPlacement(char[,] matrix, int x, int y) {
        if (matrix[x, y] == 'P')
        {
            //Debug.Log(matrix[x, y]);
            //Debug.Log(x + " " + y);
            return false;
        }


        int rows = matrix.GetLength(0);
        int columns = matrix.GetLength(1);
        // (x-1, y-1) (x-1, y) (x-1, y+1)
        // (x, y-1) (x,y) (x, y+1)
        // (x+1, y-1) (x+1,y) (x+1, y+1)

        char[] charArray = { 'D', 'd' };
        if (charArray.Contains(matrix[x, y]))
        {
            return true;
        }


        if (x > 0 && y > 0 && charArray.Contains(matrix[x - 1, y - 1])  && charArray.Contains(matrix[x - 1, y])  && charArray.Contains(matrix[x, y - 1]) )
        {
            return false;
        }

        if (x > 0 && y + 1 < columns && charArray.Contains(matrix[x - 1, y])  && charArray.Contains(matrix[x - 1, y + 1])  && charArray.Contains(matrix[x, y + 1]) )
        {
            return false;
        }

        if (x + 1 < rows && y + 1 < columns && charArray.Contains(matrix[x, y + 1])  && charArray.Contains(matrix[x + 1, y] ) && charArray.Contains(matrix[x + 1, y + 1]) )
        {
            return false;
        }

        if (x + 1 < rows && y > 0 && charArray.Contains(matrix[x, y - 1])  && charArray.Contains(matrix[x + 1, y - 1])  && charArray.Contains(matrix[x + 1, y]) )
        {
            return false;
        }

        return true;


        if (y > 0 && x > 0 && x + 1 < rows && y + 1 < columns) {
            //Stanga jos
            if (matrix[x, y] == '0' && charArray.Contains(matrix[x - 1, y]) && charArray.Contains(matrix[x, y + 1]) && charArray.Contains(matrix[x - 1, y + 1])) {
                return false;
            }

            //Stanga sus
            if (matrix[x, y] == '0' && charArray.Contains(matrix[x + 1, y]) && charArray.Contains(matrix[x, y + 1]) && charArray.Contains(matrix[x + 1, y + 1])) {
                return false;
            }
            //Dreapta sus
            if (matrix[x, y] == '0' && charArray.Contains(matrix[x, y - 1]) && charArray.Contains(matrix[x + 1, y]) && charArray.Contains(matrix[x + 1, y - 1])) {
                return false;
            }
            //Dreapta jos
            if (matrix[x, y] == '0' && charArray.Contains(matrix[x - 1, y]) && charArray.Contains(matrix[x - 1, y - 1]) && charArray.Contains(matrix[x, y - 1])) {
                return false;
            }
        }

        return true;
    }

    public static bool ShortestPath(char[,] matrix, int startX, int startY, int targetX, int targetY, out List<(int, int)> path)
    {
        int rows = matrix.GetLength(0);
        int columns = matrix.GetLength(1);

        bool[,] visited = new bool[rows, columns];
        int[,] distance = new int[rows, columns];
        int[,] parentX = new int[rows, columns];
        int[,] parentY = new int[rows, columns];

        // Initialize visited, distance, parentX, and parentY arrays
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                visited[i, j] = false;
                distance[i, j] = int.MaxValue;
                parentX[i, j] = -1;
                parentY[i, j] = -1;
            }
        }

        // Mark the starting point as visited and initialize its distance as 0
        visited[startX, startY] = true;
        distance[startX, startY] = 0;

        Queue<(int, int)> queue = new Queue<(int, int)>();
        queue.Enqueue((startX, startY));

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            int x = current.Item1;
            int y = current.Item2;

            

            // If the target point is reached, construct the path and return true
            if (x == targetX && y == targetY)
            {
                ConstructPath(matrix, parentX, parentY, x, y, out path);

                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        if (matrix[i, j] == 'd')
                        {
                            matrix[i, j] = '0';
                        }
                    }
                }

                return true;
            }

            // Explore the neighbors of the current point
            for (int i = 0; i < 4; i++)
            {
                int nx = x + dx[i];
                int ny = y + dy[i];

                // Check if the neighbor is within the matrix bounds and is not an obstacle
                if (nx >= 0 && nx < rows && ny >= 0 && ny < columns && matrix[nx, ny] != 'P' && !visited[nx, ny] && checkRoadPlacement(matrix, nx, ny))
                {
                    if (matrix[nx, ny] != 'D')
                        matrix[nx, ny] = 'd';
                    visited[nx, ny] = true;
                    distance[nx, ny] = distance[x, y] + 1;
                    parentX[nx, ny] = x;
                    parentY[nx, ny] = y;
                    queue.Enqueue((nx, ny));
                }
            }
        }

        // If the target point is not reachable, return false and an empty path
        path = new List<(int, int)>();

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                if (matrix[i, j] == 'd')
                {
                    matrix[i, j] = '0';
                }
            }
        }

        return false;
    }

    private static void ConstructPath(char[,] matrix, int[,] parentX, int[,] parentY, int targetX, int targetY, out List<(int, int)> path)
    {
        path = new List<(int, int)>();
        int x = targetX;
        int y = targetY;

        while (x != -1 && y != -1)
        {
            
            path.Add((x, y));
            int px = parentX[x, y];
            int py = parentY[x, y];
            x = px;
            y = py;
            
        }

        path.Reverse(); // Reverse the path to get the correct order
    }

    public static void getPath(char[,] matrix, int startX, int startY, int targetX, int targetY)
    {
        List<(int, int)> shortestPath;

        string output = "";
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                output += matrix[i, j] + " ";
            }
            output += "\n";
        }
        //Debug.Log(output);

        bool pathExists = ShortestPath(matrix, startX, startY, targetX, targetY, out shortestPath);

        if (!pathExists)
        {
            Debug.Log("Target point is not reachable.");
        }
        else
        {
            foreach (var point in shortestPath)
            {
                int x = point.Item1;
                int y = point.Item2;
                matrix[x, y] = 'K';
            }

            //Debug.Log("Modified Matrix:");

            output = "";
            for (int i = 0; i <  matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    output += matrix[i, j] + " ";
                }
                output += "\n";
            }
            //Debug.Log(output);

            foreach (var point in shortestPath)
            {
                int x = point.Item1;
                int y = point.Item2;
                matrix[x, y] = 'D';
            }
        }
    }
}
