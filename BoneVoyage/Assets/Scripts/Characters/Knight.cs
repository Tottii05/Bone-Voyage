using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kight : ACharacter
{
    public List<GameObject> weapons;
    public GameObject currentWeapon;

    public List<GameObject> shields;
    public GameObject currentShield;

    public bool reduceDamage = false;
    public void Start()
    {
        characterBehaviour = GetComponent<CharacterBehaviour>();
        animator = GetComponent<Animator>();
        checkPoint = transform.position;

        foreach (GameObject weapon in weapons)
        {
            weapon.SetActive(false);
        }
        foreach (GameObject shield in shields)
        {
            shield.SetActive(false);
        }

        currentWeapon = weapons[0];
        currentWeapon.SetActive(true);
        currentShield = shields[0];
        currentShield.SetActive(true);
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
            animator.SetTrigger("startBlocking");
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
        characterBehaviour.usingSpecial = true;
        animator.SetBool("special", characterBehaviour.usingSpecial);
        animator.SetTrigger("useSpecial");
        currentWeapon.GetComponent<BoxCollider>().enabled = true;
        StartCoroutine(PerformSpecial());
    }
    public IEnumerator PerformAttack()
    {
        characterBehaviour.isWaiting = false;
        animator.SetTrigger("attack");
        yield return new WaitForSeconds(0.1f);
        currentWeapon.GetComponent<BoxCollider>().enabled = true;
        yield return new WaitForSeconds(0.1f);
        currentWeapon.GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(0.1f);
        characterBehaviour.isWaiting = true;
    }
    public IEnumerator PerformSpecial()
    {
        yield return new WaitForSeconds(5f);
        characterBehaviour.usingSpecial = false;
        animator.SetBool("special", characterBehaviour.usingSpecial);
        currentWeapon.GetComponent<BoxCollider>().enabled = false;
    }
    public override void TakeDamage(float damage)
    {
        if (reduceDamage)
        {
            damage *= currentShield.GetComponent<Shield>().damageReduction;
            animator.SetTrigger("blockHit");
        }
        else
        {
            animator.SetTrigger("hit");
        }
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }
}
