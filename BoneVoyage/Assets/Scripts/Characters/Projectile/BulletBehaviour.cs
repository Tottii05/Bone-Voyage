using System.Collections;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 20f;
    private Rigidbody rb;
    private Mage mage;

    public void Initialize(Mage mageRef)
    {
        mage = mageRef;
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
        StartCoroutine(DeactivateAfterTime());
    }

    private IEnumerator DeactivateAfterTime()
    {
        yield return new WaitForSeconds(mage.bulletLifeTime);
        mage.ReturnBulletToPool(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        mage.ReturnBulletToPool(gameObject);
    }
}
