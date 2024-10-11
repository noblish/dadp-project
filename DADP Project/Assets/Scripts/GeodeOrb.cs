using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeodeOrb : MonoBehaviour
{
    
    public GameObject orb;
    public Transform orbSpot;
    // Start is called before the first frame update
    void Start()
    {
        orb.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Pickaxe")
        {
            other.gameObject.transform.parent = null;
            other.gameObject.SetActive(false);
            orb.SetActive(true);
            orb.transform.position = orbSpot.position;
            Debug.Log("Orb Freed!");
        }
    }
}
