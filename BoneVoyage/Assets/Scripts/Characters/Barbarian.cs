using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barbarian : ACharacter
{
    public BoxCollider attackCollider;
    public GameObject mug;
    public List<GameObject> weaponsLeft ;
    public List<GameObject> weaponsRight;
    public GameObject currentWeaponLeft;
    public GameObject currentWeaponRight;
    public int damageAugment = 5;
    public int speedAugment = 2;
    public int heal=20;
    public float mugbuffDuration = 5f;
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
        StartCoroutine(PerformSupport());
    }
    public override void Special()
    {
        StartCoroutine(PerformSpecial());
    }
    public IEnumerator PerformAttack()
    {
        characterBehaviour.isWaiting = false;
        animator.SetTrigger("Attack");
        StartCoroutine(Combo());

        yield return new WaitForSeconds(0.7f);

        characterBehaviour.isWaiting = true;
    }

    public IEnumerator PerformSupport()
    {
        characterBehaviour.isWaiting = false;
        animator.SetTrigger("Support");
        mug.SetActive(true);
        currentWeaponLeft.SetActive(false);
        currentWeaponRight.SetActive(false);
        
        yield return new WaitForSeconds(2f);
        
        StartCoroutine(MugBuff());
        mug.SetActive(false);
        currentWeaponLeft.SetActive(true);
        currentWeaponRight.SetActive(true);
        characterBehaviour.isWaiting = true;
    }

    public IEnumerator PerformSpecial()
    {
        characterBehaviour.isWaiting = false;
        animator.SetTrigger("Special");

        yield return new WaitForSeconds(0.7f);

        characterBehaviour.isWaiting = true;
    }
    public IEnumerator Hit()
    {
        characterBehaviour.isWaiting = false;
        animator.SetTrigger("Hit");

        yield return new WaitForSeconds(0.3f);

        characterBehaviour.isWaiting = true;
    }
    public IEnumerator Combo()
    {
        int num=animator.GetInteger("AttackNum");
        switch (num)
        {
            case 0:
                animator.SetInteger("AttackNum", 1);
                break;
            case 1:
                animator.SetInteger("AttackNum", 2);
                break;
            case 2:
                animator.SetInteger("AttackNum", 3);
                break;
            case 3:
                animator.SetInteger("AttackNum", 1);
                break;
        }
        
        yield return new WaitForSeconds(1.5f);
        if(num+1== animator.GetInteger("AttackNum"))
        {
            animator.SetInteger("AttackNum", 0);
        }
        
    }
    public IEnumerator MugBuff()
    {
        health += heal;
        speed += speedAugment;
        yield return new WaitForSeconds(mugbuffDuration);
        speed -= speedAugment;
    }
    public new void TakeDamage(float damage)
    {
        
        base.TakeDamage(damage);
        StartCoroutine(Hit());
    }
}
