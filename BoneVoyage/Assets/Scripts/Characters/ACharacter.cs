using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public Slider healthBar;

    public abstract void Attack();
    public abstract void Support();
    public abstract void Special();

    public virtual void TakeDamage(float damage)
    {
        if (!isDead)
        {
            animator.SetTrigger("hit");
            health -= damage;
            healthBar.value = health / 100;
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
        healthBar.value = 1;
    }
}