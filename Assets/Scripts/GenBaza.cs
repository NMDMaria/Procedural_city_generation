using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GenBaza : MonoBehaviour
{
    public GameObject baza;
    public GameObject parc;

    // Start is called before the first frame update
    void Start()
    {
       baza = GameObject.FindGameObjectsWithTag("Baza");
       parc = GameObject.FindGameObjectsWithTag("Parc");

       int x = parc.GetComponent<Parcuri>().x;
       int y = parc.GetComponent<Parcuri>().y;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
