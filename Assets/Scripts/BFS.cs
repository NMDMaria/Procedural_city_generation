using System;
using System.Collections.Generic;
using UnityEngine;

public class BFS: MonoBehaviour
{
    private static int[] dx = { -1, 0, 1, 0 }; // Offsets for moving in four directions: up, right, down, left
    private static int[] dy = { 0, 1, 0, -1 };

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
                ConstructPath(parentX, parentY, x, y, out path);
                return true;
            }

            // Explore the neighbors of the current point
            for (int i = 0; i < 4; i++)
            {
                int nx = x + dx[i];
                int ny = y + dy[i];

                // Check if the neighbor is within the matrix bounds and is not an obstacle
                if (nx >= 0 && nx < rows && ny >= 0 && ny < columns && matrix[nx, ny] != 'P' && !visited[nx, ny])
                {
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
        return false;
    }

    private static void ConstructPath(int[,] parentX, int[,] parentY, int targetX, int targetY, out List<(int, int)> path)
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
                matrix[x, y] = 'D';
            }

            Debug.Log("Modified Matrix:");

            string output = "";
            for (int i = 0; i <  matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    output += matrix[i, j] + " ";
                }
                output += "\n";
            }
            Debug.Log(output);
        }
    }
}
