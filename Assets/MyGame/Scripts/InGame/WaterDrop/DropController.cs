using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DropController : MonoBehaviour
{
    private DropPortManagerStatus _dropPortManagerStatus;
    public float ReinForceAmount => _dropPortManagerStatus.ReinForceAmount;
    public float ReinForceTime => _dropPortManagerStatus.ReinForceTime;

    public void SetDrop(DropPortManagerStatus dropPortManagerStatus)
    {
        _dropPortManagerStatus = dropPortManagerStatus;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Floor"))
        {
            Destroy(this.gameObject);
        }
    }
}
