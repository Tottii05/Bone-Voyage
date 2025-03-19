using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kight : ACharacter
{
    public BoxCollider attackCollider;
    public bool reduceDamage = false;
    public float damageReduction = 0.5f;
    public void Start()
    {
        characterBehaviour = GetComponent<CharacterBehaviour>();
        animator = GetComponent<Animator>();
        checkPoint = transform.position;
    }

    public override void Attack()
    {
        StartCoroutine(PerformAttack());
    }
    public override void Support()
    {
        reduceDamage = !reduceDamage;
        if (reduceDamage)
        {
            characterBehaviour.isWaiting = false;
            animator.SetBool("blocking", true);
        }
        else
        {
            animator.SetBool("blocking", false);
            characterBehaviour.isWaiting = true;
        }
    }
    public override void Special()
    {
    }
    public IEnumerator PerformAttack()
    {
        yield return null;
    }
    public new void TakeDamage(float damage)
    {
        if (reduceDamage)
        {
            damage *= damageReduction;
        }
        base.TakeDamage(damage);
    }
}
