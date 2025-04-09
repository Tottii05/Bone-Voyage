using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEBehaviour : MonoBehaviour
{
    public float damagePerTick = 14f;
    public float tickInterval = 1f;
    public LayerMask enemyLayerMask; // Asigna solo la capa de los enemigos en el Inspector
    private List<IDamageable> enemiesInRange = new List<IDamageable>();
    private GameObject mageParent;

    void Start()
    {
        mageParent = transform.parent.gameObject;
        if (mageParent == null)
        {
            Debug.LogError("No parent found for AOE! Ensure this is a child of the mage.");
        }
        else
        {
            Debug.Log("Mage parent assigned: " + mageParent.name);
        }
        CheckInitialEnemies();
        StartCoroutine(ApplyDamageOverTime());
    }

    private void CheckInitialEnemies()
    {
        CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
        if (capsuleCollider == null)
        {
            Debug.LogError("No CapsuleCollider found on AOE object!");
            return;
        }
        float radius = capsuleCollider.radius;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, enemyLayerMask);
        Debug.Log($"Checked for enemies at spawn. Found {hitColliders.Length} colliders in layer mask {LayerMask.LayerToName(enemyLayerMask.value)}.");
        foreach (Collider hit in hitColliders)
        {
            IDamageable damageable = hit.GetComponent<IDamageable>();
            if (damageable != null && hit.gameObject != mageParent && !enemiesInRange.Contains(damageable))
            {
                enemiesInRange.Add(damageable);
                Debug.Log("Enemy detected at spawn: " + damageable);
            }
        }
    }

    private IEnumerator ApplyDamageOverTime()
    {
        float elapsedTime = 0f;

        while (elapsedTime < 5f)
        {
            Debug.Log($"Tick at {elapsedTime}s. Enemies in range: {enemiesInRange.Count}");
            if (enemiesInRange.Count == 0)
            {
                Debug.Log("No enemies in range to damage.");
            }
            foreach (IDamageable enemy in new List<IDamageable>(enemiesInRange))
            {
                Debug.Log("Processing enemy: " + enemy);
                if (enemy == null)
                {
                    Debug.Log("Enemy is null. Removing from list.");
                    enemiesInRange.Remove(enemy);
                    continue;
                }

                if (IsAlive(enemy))
                {
                    MonoBehaviour enemyMono = enemy as MonoBehaviour;
                    if (enemyMono == null)
                    {
                        Debug.Log("Enemy is not a MonoBehaviour. Skipping.");
                        continue;
                    }

                    if (enemyMono.gameObject == mageParent)
                    {
                        Debug.Log("Enemy is the mage. Skipping damage.");
                        continue;
                    }

                    Debug.Log($"Applying {damagePerTick} damage to enemy: {enemy}");
                    enemy.TakeDamage(damagePerTick);

                    EnemyController enemyGO = enemyMono.GetComponent<EnemyController>();
                    if (enemyGO != null)
                    {
                        enemyGO.damageRecieved = damagePerTick;
                        Debug.Log($"EnemyController found. HP remaining: {enemyGO.HP}");
                    }
                    else
                    {
                        Debug.Log("No EnemyController found on this enemy.");
                    }
                }
                else
                {
                    Debug.Log("Enemy is not alive. Removing from list.");
                    enemiesInRange.Remove(enemy);
                }
            }

            yield return new WaitForSeconds(tickInterval);
            elapsedTime += tickInterval;
        }
        Debug.Log("AOE duration ended. Destroying AOE.");
        Destroy(gameObject);
    }

    private bool IsAlive(IDamageable enemy)
    {
        MonoBehaviour enemyMono = enemy as MonoBehaviour;
        if (enemyMono == null)
        {
            Debug.Log("Enemy is not a MonoBehaviour.");
            return false;
        }

        EnemyController ec = enemyMono.GetComponent<EnemyController>();
        if (ec != null)
        {
            Debug.Log($"Enemy HP: {ec.HP}");
            return ec.HP > 0;
        }

        Debug.Log("No EnemyController found. Assuming enemy is alive.");
        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered by: " + other.gameObject.name);
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null && other.gameObject != mageParent && !enemiesInRange.Contains(damageable))
        {
            enemiesInRange.Add(damageable);
            Debug.Log("Enemy entered AOE: " + damageable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            enemiesInRange.Remove(damageable);
            Debug.Log("Enemy exited AOE: " + damageable);
        }
    }
}