using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class closeDoor : MonoBehaviour
{
    private Animator animator;
    void Start()
    {
        animator = GetComponentInParent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (animator != null && other.CompareTag("Player"))
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            // Verifica si el estado actual NO es "Closed"
            if (!stateInfo.IsName("doorCastleClose"))
            {
                animator.SetTrigger("close");
            }
        }
    }
}
