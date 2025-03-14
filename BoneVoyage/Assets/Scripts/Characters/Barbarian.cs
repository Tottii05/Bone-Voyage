using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barbarian : ACharacter
{
    public BoxCollider attackCollider;
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
    }
    public override void Special()
    {
    }
    public IEnumerator PerformAttack()
    {
        yield return null;
    }
}
