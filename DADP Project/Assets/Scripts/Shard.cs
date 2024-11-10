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
    public FirstPersonControls player;

    private void Start()
    {
        GetComponent<MeshRenderer>().sharedMaterial = ShardMaterial;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Lava"))
        {
            Debug.Log("Hot!");
            player.StartCoroutine(player.Message("Shard Heated"));
            GetComponent<MeshRenderer>().sharedMaterial = ShardHeatedMaterial;
            shardState = true;
        }
        else if (other.gameObject.name == "Mushroom Orb Trigger" && shardState)
        {
            player.toolState = true;
            Debug.Log("Orb!");
        }
    }

    public bool getShardState()
    {
        return shardState;
    }

    

}
