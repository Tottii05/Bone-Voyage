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
            if (isEnemy)
            {
                if (other.gameObject.CompareTag("Player"))
                {
                    other.gameObject.GetComponent<ACharacter>().TakeDamage(damage);
                }
                else if (other.gameObject.CompareTag("DestroyableTree"))
                {
                    other.gameObject.GetComponent<DestroyableTree>().GetHit(gameObject, damage);
                }
            }
            else
            {
                if (other.gameObject.CompareTag("Enemy"))
                {
                    other.gameObject.GetComponent<EnemyController>().damageRecieved = damage;
                    other.gameObject.GetComponent<EnemyController>().TakeDamage(damage);
                }
                else if (other.gameObject.CompareTag("DestroyableTree"))
                {
                    other.gameObject.GetComponent<DestroyableTree>().GetHit(gameObject, damage);
                }
            }
        }
    }
}
