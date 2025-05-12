using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openDoor : MonoBehaviour
{
    private Animator animator;
    void Start()
    {
        animator = GetComponentInParent<Animator>();
        Debug.Log("Animator component found: " + animator);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the trigger zone.");
            animator.SetTrigger("open");
            Destroy(gameObject);
        }
    }
}
