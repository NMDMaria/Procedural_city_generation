using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabLoader : MonoBehaviour
{

    public GameObject road;

    // Start is called before the first frame update
    void Start()
    {
        road = Resources.Load<GameObject>("road_black");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
