using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockResetTrigger : MonoBehaviour
{
    public RockManager rockManager;
    public BoxCollider trigger;

    public void Start()
    {
        trigger = GetComponent<BoxCollider>();
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            rockManager.ResetRockMatrix();
        }
    }
}
