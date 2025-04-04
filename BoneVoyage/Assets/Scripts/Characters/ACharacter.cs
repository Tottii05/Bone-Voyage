using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ACharacter : MonoBehaviour, IDamageable
{
    public CharacterBehaviour characterBehaviour;
    public Animator animator;
    public Vector3 checkPoint;
    [Header("Character variables")]
    [Header("----------------------------------------")]
    public float health = 100;
    public float speed = 5f;
    public bool isDead = false;

    public abstract void Attack();
    public abstract void Support();
    public abstract void Special();

    public virtual void TakeDamage(float damage)
    {
        if (!isDead)
        {
            animator.SetTrigger("hit");
            health -= damage;
            if (health <= 0)
            {
                Die();
            }
        }
    }

    public void Die()
    {
        isDead = true;
        characterBehaviour.rb.velocity = Vector3.zero;
        animator.SetTrigger("die");
        StartCoroutine(Respawn());
    }

    public IEnumerator Respawn()
    {
        yield return new WaitForSeconds(2.5f);
        transform.position = checkPoint;
        health = 100;
        animator.SetTrigger("respawn");
        isDead = false;
        characterBehaviour.isFrozen = false;
    }
}