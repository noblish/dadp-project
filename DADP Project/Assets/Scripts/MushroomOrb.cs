using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MushroomOrb : MonoBehaviour
{
    public GameObject orb;
    public Transform orbSpot;

    public TextMeshProUGUI promptUI;
    public String pickupText;
    private void Start()
    {
        orb.SetActive(false);
    }

    

    public void ToolUsage(GameObject other)
    {
        if (other.gameObject.name == "Crystal Shard")
        {
            if (other.GetComponent<Shard>().getShardState())
            {
                orb.SetActive(true);
                orb.transform.position = orbSpot.position;
                Debug.Log("Orb Freed!");
                other.transform.parent = null;
                other.gameObject.SetActive(false);
            }
            else
            {
                promptUI.gameObject.SetActive(true); 
                promptUI.text = "Need to heat this up!";
            }
        }
    }
}
