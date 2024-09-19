using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageArea : MonoBehaviour
{
    public float damageTime;
    private float damageTimeRemaining;
    private bool inArea;
    private bool inAreaObject;
    private GameObject player;
    private GameObject heldObject1;
    private GameObject heldObject2;

    // Start is called before the first frame update
    void Start()
    {
        damageTimeRemaining = damageTime;
        inArea = false;
        inAreaObject = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (inArea)
        {
            damageTimeRemaining -= Time.deltaTime;
            Debug.Log(damageTimeRemaining + " Seconds");
            if (damageTimeRemaining <= 0f)
            {
                player.GetComponent<FirstPersonControls>().CheckpointTeleport();
                //player.transform.position = player.GetComponent<FirstPersonControls>().lastCheckpoint.position;
                Debug.Log("TELEPORT = " + player.transform.ToString() + " TO "
                          + player.GetComponent<FirstPersonControls>().lastCheckpoint);
            }
        }
        if (inAreaObject)
        {
            damageTimeRemaining -= Time.deltaTime;
            if (damageTimeRemaining <= 0f)
            {
                heldObject1.GetComponent<Teleport>().CheckpointTeleport();
                heldObject2.GetComponent<Teleport>().CheckpointTeleport();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inArea = true;
            player = other.gameObject;
        }
        if (other.GetComponent<Teleport>())
        {
            inAreaObject = true;
            if (heldObject1 != null)
            {
                heldObject2 = other.gameObject;
            }
            else
            {
                heldObject1 = other.gameObject;
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inArea = false;
            damageTimeRemaining = damageTime;
        }

        if (other.GetComponent<Teleport>())
        {
            inAreaObject = false;
        }
    }
}

