using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DropController : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Floor"))
        {
            Destroy(this.gameObject);
        }
    }
}
