using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class HandOrb : MonoBehaviour
{
    
    public GameObject orb;
    public Transform orbSpot;
    public TextMeshProUGUI promptUI;
    public GameObject[] coinArr = new GameObject[2];
    public int coinCount = 0;
    public string pickupText;
    // Start is called before the first frame update
    void Start()
    {
        orb.SetActive(false);
        coinArr[0].SetActive(false);
        coinArr[1].SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToolUsage(GameObject other)
    {
        if (other.name == "Coin")
        {
            other.gameObject.transform.parent = null;
            other.gameObject.SetActive(false);
            coinCount += 1;
            coinArr[0].SetActive(true);
            if (coinCount == 2)
            {
                orb.SetActive(true);
                orb.transform.position = orbSpot.position;
                Debug.Log("Orb Freed!");
                coinArr[1].SetActive(true);
            }
        }
    }
}
