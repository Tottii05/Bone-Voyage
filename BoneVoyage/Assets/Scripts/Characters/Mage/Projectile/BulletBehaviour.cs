using System.Collections;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 20f;
    private Rigidbody rb;
    private Mage mage;
    private RangedAttack ranged;

    public void Initialize(Mage mageRef)
    {
        mage = mageRef;
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
        StartCoroutine(DeactivateAfterTime());
    }

    public void Initialize(RangedAttack rangedAttack)
    {
        ranged = rangedAttack;
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
        StartCoroutine(DeactivateAfterTime());
    }

    private IEnumerator DeactivateAfterTime()
    {
        yield return new WaitForSeconds(2);
        if (mage != null)
            mage.ReturnBulletToPool(gameObject);
        else
            ranged.ReturnBulletToPool(gameObject);
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Bullet hit: " + other.gameObject.name);
        if (mage != null)
        {
            mage.ReturnBulletToPool(gameObject);
            Debug.Log("Bullet hit: " + other.gameObject.name);
            if (other.gameObject.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(damage);
                other.gameObject.GetComponent<EnemyController>().damageRecieved = damage;
            }
        }
        else
        {
            ranged.ReturnBulletToPool(gameObject);
            Debug.Log("Bullet hit: " + other.gameObject.name);
            if (other.gameObject.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(damage);
            }
        }
    }
}
