using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DropController : MonoBehaviour
{
    private Drop _drop;
    public float ReinForceAmount => _drop.ReinForceAmount;
    public float ReinForceTime => _drop.ReinForceTime;

    public void SetDrop(Drop drop)
    {
        _drop = drop;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Floor"))
        {
            Destroy(this.gameObject);
        }
    }
}
