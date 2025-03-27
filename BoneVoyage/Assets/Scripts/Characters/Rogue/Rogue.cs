using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rogue : ACharacter
{
    [Header("Prefabs and projectiles spawnpoint")]
    [Header("----------------------------------------")]
    public GameObject bulletPrefab;
    public GameObject grenadePrefab;
    public GameObject specialAura;
    public Transform spawnPoint;
    [Header("Combat variables and CDs")]
    [Header("----------------------------------------")]
    private bool usingSpecial = false;
    private bool specialCD = false;
    private bool dashCD = false;
    public float specialDuration;
    public float specialCDTime;
    public float arrowCD;
    public float grenadeCD;
    public float rollCD;
    [Header("Pool variables")]
    [Header("----------------------------------------")]
    public Stack<GameObject> arrowStack = new Stack<GameObject>();
    public int arrowPoolSize = 3;
    public float arrowLifeTime = 2f;
    public Stack<GameObject> grenadeStack = new Stack<GameObject>();
    public int grenadePoolSize = 5;
    public float grenadeLifeTime = 3f;
    
    public void Start()
    {
        specialAura.SetActive(false);
        characterBehaviour = GetComponent<CharacterBehaviour>();
        animator = GetComponent<Animator>();
        checkPoint = transform.position;
        CreateArrowPool();
        CreateGrenadePool();
    }

    public override void Attack()
    {
        StartCoroutine(PerformAttack()); 
    }
    public override void Support()
    {   
        if (!dashCD) StartCoroutine(PerformRoll());
    }
    public override void Special()
    {
        if (!specialCD)
        {
            StartCoroutine(useEspecial());
        }
    }

    public void CreateArrowPool()
    {
        for (int i = 0; i < arrowPoolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            arrowStack.Push(bullet);
        }
    }

    public void CreateGrenadePool()
    {
        for (int i = 0; i < grenadePoolSize; i++)
        {
            GameObject grenade = Instantiate(grenadePrefab);
            grenade.SetActive(false);
            grenadeStack.Push(grenade);
        }
    }

    public void ReturnArrowToPool(GameObject bullet)
    {
        bullet.SetActive(false);
        arrowStack.Push(bullet);
    }

    public void ReturnGrenadeToPool(GameObject grenade)
    {
        grenade.SetActive(false);
        grenadeStack.Push(grenade);
    }
    public IEnumerator PerformAttack()
    {         
        if (!usingSpecial)
        {
            if (arrowStack.Count > 0)
            {
                characterBehaviour.isWaiting = false;
                animator.SetTrigger("Attack");
                yield return new WaitForSeconds(0.5f);
                if (bulletPrefab != null)
                {
                    GameObject bullet = arrowStack.Pop();
                    bullet.SetActive(true);
                    bullet.transform.position = spawnPoint.position;
                    bullet.transform.rotation = spawnPoint.rotation;
                    bullet.GetComponent<ArrowBehaviour>().Initialize(this);
                }
                yield return new WaitForSeconds(arrowCD);
            }            
        } else
        {
            characterBehaviour.isWaiting = false;
            animator.SetTrigger("Attack");
            yield return new WaitForSeconds(0.7f);
            if (grenadePrefab != null)
            {
                GameObject grenade = grenadeStack.Pop();
                grenade.SetActive(true);
                grenade.transform.position = spawnPoint.position;
                grenade.transform.rotation = spawnPoint.rotation;
                grenade.GetComponent<GrenadeBehaviour>().Initialize(this);
            }
            yield return new WaitForSeconds(grenadeCD);
        }      
        characterBehaviour.isWaiting = true;
    }

    public IEnumerator PerformRoll()
    {
        characterBehaviour.isWaiting = false;
        dashCD = true;
        animator.SetTrigger("Dash");
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.detectCollisions = false;
        rb.velocity = Vector3.zero;

        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + transform.forward * 4f; // Ajusta el valor de +Xf para la distancia del roll.

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
        characterBehaviour.isWaiting = true;
        yield return new WaitForSeconds(rollCD);
        dashCD = false; 

    }

    public IEnumerator useEspecial()
    {
        specialAura.SetActive(true);
        usingSpecial = true;
        specialCD = true;
        yield return new WaitForSeconds(specialDuration);
        specialAura.SetActive(false);
        usingSpecial = false;
        yield return new WaitForSeconds(specialCDTime);
        specialCD = false;
    }

}
