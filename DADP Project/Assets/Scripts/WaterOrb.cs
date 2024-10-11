using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterOrb : MonoBehaviour
{
    public GameObject orb;
    public Transform orbSpot;

    private void Start()
    {
        orb.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Stone Tablet")
        {
            other.gameObject.transform.parent = null;
            other.gameObject.SetActive(false);
            orb.SetActive(true);
            orb.transform.position = orbSpot.position;
            Debug.Log("Orb Freed!");
        }
    }
}
