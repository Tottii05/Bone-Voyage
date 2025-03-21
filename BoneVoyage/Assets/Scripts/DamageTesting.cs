using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTesting : MonoBehaviour, IDamageable
{
    public float health = 100;
    public GameObject player;
    public void Start()
    {
        Debug.Log("Health: " + health);
    }
    public void Update()
    {
        DoDamage();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("Health: " + health);
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public void DoDamage()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (player.gameObject.name == "Mage")
            {
                player.GetComponent<Mage>().TakeDamage(10);
            }
        }
    }
}
