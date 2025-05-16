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
                    if (other.gameObject.TryGetComponent(out EnemyController enemyController))
                    {
                        enemyController.damageRecieved = damage;
                    }
                    else if (other.gameObject.TryGetComponent(out VillageBoss villageBoss))
                    {
                        villageBoss.damageRecieved = damage;
                    }
                    IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
                    damageable.TakeDamage(damage);
                }
                else if (other.gameObject.CompareTag("DestroyableTree"))
                {
                    other.gameObject.GetComponent<DestroyableTree>().GetHit(gameObject, damage);
                }
            }
        }
    }
}
