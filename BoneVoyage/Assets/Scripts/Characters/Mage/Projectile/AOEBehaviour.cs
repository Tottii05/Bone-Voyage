using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class AOEBehaviour : MonoBehaviour
{
    public float damagePerTick = 14f;
    public float tickInterval = 1f;
    private CapsuleCollider capsuleCollider;
    private HashSet<EnemyController> damagedEnemies = new HashSet<EnemyController>();

    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        if (capsuleCollider == null) Destroy(gameObject);
        StartCoroutine(ApplyDamageOverTime());
    }

    private IEnumerator ApplyDamageOverTime()
    {
        float elapsedTime = 0f;

        while (elapsedTime < 5f)
        {
            damagedEnemies.Clear();
            Collider[] enemies = Physics.OverlapSphere(transform.position, capsuleCollider.radius);

            foreach (Collider enemy in enemies)
            {
                if (enemy.CompareTag("Enemy"))
                {
                    EnemyController enemyController = enemy.GetComponent<EnemyController>();
                    if (enemyController != null && enemyController.HP > 0 && !damagedEnemies.Contains(enemyController))
                    {
                        enemyController.damageRecieved = damagePerTick;
                        enemyController.TakeDamage(damagePerTick);
                        damagedEnemies.Add(enemyController);
                    }
                }
            }

            yield return new WaitForSeconds(tickInterval);
            elapsedTime += tickInterval;
        }

        Destroy(gameObject);
    }
}