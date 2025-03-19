using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ACharacter : MonoBehaviour, IDamageable
{
    public CharacterBehaviour characterBehaviour;
    public Animator animator;
    public Vector3 checkPoint;

    public float health = 100;
    public float baseDamage = 10;
    public float speed = 5f;

    public abstract void Attack();
    public abstract void Support();
    public abstract void Special();
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        animator.SetTrigger("Die");
        StartCoroutine(Respawn());
    }
    public IEnumerator Respawn()
    {
        yield return new WaitForSeconds(2.5f);
        transform.position = checkPoint;
        health = 100;
        animator.SetTrigger("Respawn");
    }
}
