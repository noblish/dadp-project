using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GeodeOrb : MonoBehaviour
{
    
    public GameObject orb;
    public Transform orbSpot;

    public TextMeshProUGUI promptUI;

    public string pickupText;
    // Start is called before the first frame update
    void Start()
    {
        orb.SetActive(false);
    }

    public void ToolUsage(GameObject other)
    {
        if (other.name == "Pickaxe")
        {
            other.gameObject.transform.parent = null;
            other.gameObject.SetActive(false);
            orb.SetActive(true);
            orb.transform.position = orbSpot.position;
            Debug.Log("Orb Freed!");
        }
    }
}
