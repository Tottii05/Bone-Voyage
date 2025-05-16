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
        if (animator!= null &&other.CompareTag("Player"))
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

           
            if (!stateInfo.IsName("doorCastle"))
            {
                animator.SetTrigger("open");
            }
        }
    }
}
