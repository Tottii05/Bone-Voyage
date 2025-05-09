using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class torchPuzzleBehaviour : MonoBehaviour
{
    private CapsuleCollider capsuleCollider;
    public Light torchLight;

    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        torchLight = GetComponentInChildren<Light>();
    }
    public void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Enemy") && !other.gameObject.CompareTag("Player") )
        {
            Debug.Log(other.tag);
            capsuleCollider.enabled = false;
            torchLight.enabled = true;
            KeyCounter keyCounter = FindObjectOfType<KeyCounter>();
            keyCounter.IncrementKeysObtained();
        }
    }
}
