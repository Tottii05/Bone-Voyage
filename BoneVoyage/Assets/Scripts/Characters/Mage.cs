using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : ACharacter
{
    public GameObject bulletPrefab;
    public Transform spawnPoint;

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
        characterBehaviour.isWaiting = false;
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.7f);
        if (bulletPrefab != null)
        {
            Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation);
        }
        characterBehaviour.isWaiting = true;
    }
}
