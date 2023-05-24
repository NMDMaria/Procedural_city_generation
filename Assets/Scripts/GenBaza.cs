using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GenBaza : MonoBehaviour
{
    public GameObject baza;
    public GameObject parc;
    public int tileSize = 3;

    // Start is called before the first frame update
    void Start()
    {
        baza = GameObject.FindGameObjectWithTag("Baza");
        parc = GameObject.FindGameObjectWithTag("Parc");
    }

    // Update is called once per frame
    void Update()
    {
        float x = parc.GetComponent<Parcuri>().x;
        float y = parc.GetComponent<Parcuri>().y;
        baza.transform.localScale = new Vector3(tileSize*x, 1, tileSize*y);
    }
}
