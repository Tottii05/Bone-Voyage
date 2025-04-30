using System.Collections;
using UnityEngine;

public class VillageBossBullet : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 20f;
    private Rigidbody rb;
    public VillageBoss mage;
    public RangedAttack ranged;

    public void Initialize(VillageBoss mageRef)
    {
        mage = mageRef;
        rb = GetComponent<Rigidbody>();

        Collider bulletCollider = GetComponent<Collider>();
        Collider[] bossColliders = mage.GetComponentsInChildren<Collider>();
        foreach (Collider col in mage.GetComponentsInChildren<Collider>())
        {
            Physics.IgnoreCollision(bulletCollider, col);
        }

        rb.velocity = transform.rotation * Vector3.forward * speed; 
        StartCoroutine(DeactivateAfterTime());
        StartCoroutine(EnableColliderDelayed());
    }

    private IEnumerator EnableColliderDelayed()
    {
        Collider col = GetComponent<Collider>();
        col.enabled = false;
        yield return null; // espera un frame
        col.enabled = true;
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
        Debug.Log($"Bullet hit {other.name} at {Time.time}, mage: {(mage != null)}");
        if (mage != null)
        {
            mage.ReturnBulletToPool(gameObject);
            if (other.gameObject.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(damage);
            }
        }
        else
        {
            if (other.CompareTag("Enemy"))
                return;

        }
    }
}
