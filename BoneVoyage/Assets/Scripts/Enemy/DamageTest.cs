using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTest : MonoBehaviour, IDamageable
{
    public GameObject player;
    public float health = 100;
    public float baseDamage = 10;
    public float speed = 5f;
    private CapsuleCollider capsuleCollider;
    public void Start()
    {
        player = GameObject.Find("knight");
        capsuleCollider = GetComponent<CapsuleCollider>();
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
    }
    public void Die()
    {
        health = 100;
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.GetComponent<ACharacter>().TakeDamage(baseDamage);
        }
    }
}
