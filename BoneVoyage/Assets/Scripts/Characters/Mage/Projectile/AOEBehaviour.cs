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
            foreach (IDamageable enemy in enemiesInRange)
            {
                enemy?.TakeDamage(damagePerTick);
            }

            yield return new WaitForSeconds(tickInterval);
            elapsedTime += tickInterval;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null && !enemiesInRange.Contains(damageable))
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
