using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public TextMeshProUGUI supportText;
    public TextMeshProUGUI specialText;
    public float supportCooldown = 5f;
    public float specialCooldown = 10f;
    public bool supportReady = true;
    public bool specialReady = true;
    public float supportTimer;
    public float specialTimer;

    [Header("Sound effects")]
    [Header("----------------------------------------")]
    public AudioSource audioSource;
    public AudioClip supportSound;
    public AudioClip specialSound;
    public AudioClip attackSound;

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
        characterBehaviour.isFrozen = true;
        GetComponent<Collider>().enabled = false;
        characterBehaviour.rb.useGravity = false;
        characterBehaviour.rb.velocity = Vector3.zero;
        animator.SetTrigger("die");
        StartCoroutine(Respawn());
    }

    public IEnumerator Respawn()
    {
        yield return new WaitForSeconds(3.4f);
        transform.position = checkPoint;
        health = 100;
        animator.SetTrigger("respawn");
        isDead = false;
        characterBehaviour.isFrozen = false;
        healthBar.value = 1;
        GetComponent<Collider>().enabled = true;
        characterBehaviour.rb.useGravity = true;
    }
}