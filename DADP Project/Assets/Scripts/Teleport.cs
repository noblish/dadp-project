using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{

    public Transform lastCheckpoint;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Checkpoint>() != null)
        {
            lastCheckpoint = other.transform;
        }
        if (other.gameObject.GetComponent<DamageArea>() != null)
        {
            transform.position = lastCheckpoint.position;
        }
    }

    public void CheckpointTeleport()
    {
        transform.position = lastCheckpoint.position;
    }
}
