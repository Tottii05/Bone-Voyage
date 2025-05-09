using System.Collections;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public float speed = 10f;
    public float damage;
    private Rigidbody rb;
    public Mage mage;
    public RangedAttack ranged;

    public void Initialize(Mage mageRef)
    {
        mage = mageRef;
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
        damage = mageRef.activeWeapon.GetComponent<WeaponDmg>().damage;
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
        if (mage != null)
        {
            mage.ReturnBulletToPool(gameObject);
            if (other.gameObject.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(damage);
                other.gameObject.GetComponent<EnemyController>().damageRecieved = damage;
            }
        }
        else
        {
            if (other.tag != "Enemy")
            {
                ranged.ReturnBulletToPool(gameObject);
                if (other.gameObject.TryGetComponent(out IDamageable damageable) && other.tag == "Player")
                {
                    damageable.TakeDamage(damage);
                }
            }
        }
    }
}
