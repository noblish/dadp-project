using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Description : MonoBehaviour
{
    [SerializeField] private string description;
    
    // Start is called before the first frame update
    public string ReturnDescription()
    {
        return description;
    }
}
