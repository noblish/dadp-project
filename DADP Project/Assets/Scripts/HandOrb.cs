using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class HandOrb : MonoBehaviour
{
    
    public GameObject orb;
    public Transform orbSpot;

    private int coinCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        orb.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Coin")
        {
            other.gameObject.transform.parent = null;
            other.gameObject.SetActive(false);
            coinCount += 1;
            if (coinCount == 2)
            {
                orb.SetActive(true);
                orb.transform.position = orbSpot.position;
                Debug.Log("Orb Freed!");
            }
        }
    }
}
