using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rogue : ACharacter
{
    public GameObject bulletPrefab;
    public GameObject grenadePrefab;
    public Transform spawnPoint;
    public bool isAttacking = false;
    public bool isRolling = false;
    public bool usingSpecial = false;
    public bool specialCD = false;

    public void Start()
    {
        characterBehaviour = GetComponent<CharacterBehaviour>();
        animator = GetComponent<Animator>();
        checkPoint = transform.position;
    }

    public override void Attack()
    {
        if (!isAttacking) StartCoroutine(PerformAttack()); 
    }
    public override void Support()
    {
        if (!isRolling) StartCoroutine(PerformRoll());
    }
    public override void Special()
    {
        if (!usingSpecial) StartCoroutine(useEspecial());
    }
    public IEnumerator PerformAttack()
    { 
        characterBehaviour.isWaiting = false;
        if (!usingSpecial)
        {
            animator.SetTrigger("Attack");
            yield return new WaitForSeconds(0.7f);
            if (bulletPrefab != null)
            {
                Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation);
            }
        } else
        {
            animator.SetTrigger("Attack");
            yield return new WaitForSeconds(0.7f);
            if (bulletPrefab != null)
            {
                Instantiate(grenadePrefab, spawnPoint.position, spawnPoint.rotation);
            }
        }
        
        characterBehaviour.isWaiting = true;
    }

    public IEnumerator PerformRoll()
    {
        Debug.Log("Roll");
        isRolling = true;
        animator.SetTrigger("Dash");
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.detectCollisions = false;
        rb.velocity = Vector3.zero;

        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + transform.forward * 3f; // Ajusta el valor de 5f para la distancia del roll.

        float duration = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            rb.MovePosition(Vector3.Lerp(startPos, targetPos, elapsedTime / duration));

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        rb.MovePosition(targetPos);
        rb.detectCollisions = true;
        gameObject.GetComponent<CapsuleCollider>().enabled = true;
        yield return new WaitForSeconds(2f);

        isRolling = false;
        
    }

    public IEnumerator useEspecial()
    {
        usingSpecial = true;
        specialCD = true;
        yield return new WaitForSeconds(5f);
        usingSpecial = false;
        yield return new WaitForSeconds(10f);
        specialCD = false;
    }
}
