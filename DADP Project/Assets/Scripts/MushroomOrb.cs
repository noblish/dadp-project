using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomOrb : MonoBehaviour
{
    public GameObject orb;
    public Transform orbSpot;

    private void Start()
    {
        orb.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Crystal Shard")
        {
            if (other.GetComponent<Shard>().getShardState())
            {
                orb.SetActive(true);
                orb.transform.position = orbSpot.position;
                Debug.Log("Orb Freed!");
            }
            else
            {
                Debug.Log("Need to heat this up!");
            }
        }
    }
}
