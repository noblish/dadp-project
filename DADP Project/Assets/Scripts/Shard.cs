using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shard : MonoBehaviour
{
    public Material ShardMaterial;
    public Material ShardHeatedMaterial;
    private bool shardState = false;

    private void Start()
    {
        GetComponent<MeshRenderer>().sharedMaterial = ShardMaterial;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Lava"))
        {
            Debug.Log("Hot!");
            GetComponent<MeshRenderer>().sharedMaterial = ShardHeatedMaterial;
            shardState = true;
        }
        else if (other.gameObject.CompareTag("Mushroom Orb") && shardState)
        {
            Debug.Log("Orb!");
        }
    }

    public bool getShardState()
    {
        return shardState;
    }

    

}
