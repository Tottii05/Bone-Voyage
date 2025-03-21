using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSource : MonoBehaviour
{
    public bool isEnemy = false;
    public BoxCollider BoxCollider;
    public float damage = 10;
    private List<GameObject> hits = new List<GameObject>();

    public void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            Debug.Log("Hit");
            if (isEnemy)
            {
                if (other.gameObject.CompareTag("Player"))
                {
                    Debug.Log("Player hit");
                    other.gameObject.GetComponent<ACharacter>().TakeDamage(damage);
                }
            }
            else
            {
                if (other.gameObject.CompareTag("Enemy"))
                {
                    Debug.Log("Enemy hit");
                    other.gameObject.GetComponent<DamageTest>().TakeDamage(damage);
                }
            }
        }
    }
}
