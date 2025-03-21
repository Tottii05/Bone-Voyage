using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEBehaviour : MonoBehaviour
{
    public float damagePerTick = 14f;
    public float tickInterval = 1f;
    private List<IDamageable> enemiesInRange = new List<IDamageable>();

    void Start()
    {
        StartCoroutine(ApplyDamageOverTime());
    }

    private IEnumerator ApplyDamageOverTime()
    {
        float elapsedTime = 0f;

        while (elapsedTime < 5f)
        {
            foreach (IDamageable enemy in new List<IDamageable>(enemiesInRange))
            {
                if (IsAlive(enemy))
                {
                    enemy.TakeDamage(damagePerTick);
                }
                else
                {
                    enemiesInRange.Remove(enemy);
                }
            }

            yield return new WaitForSeconds(tickInterval);
            elapsedTime += tickInterval;
        }
    }

    private bool IsAlive(IDamageable enemy)
    {
        return (enemy as MonoBehaviour) != null && (enemy as DamageTesting)?.health > 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null && !enemiesInRange.Contains(damageable) && IsAlive(damageable))
        {
            enemiesInRange.Add(damageable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            enemiesInRange.Remove(damageable);
        }
    }
}
