using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeRoad : MonoBehaviour
{
    public GameObject prefab;
    public int rows = 5;
    public int columns = 5;
    public float spacing = 2f;

    void Start()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                Vector3 position = new Vector3(row * spacing, 0, column * spacing);
                Collider[] colliders = Physics.OverlapSphere(position, 1f);

                if (colliders.Length == 0)
                {
                    Instantiate(prefab, position, Quaternion.identity);
                }
            }
        }
    }
}
