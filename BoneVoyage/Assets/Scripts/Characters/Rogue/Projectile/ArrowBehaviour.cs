using System.Collections;
using UnityEngine;

public class ArrowBehaviour : MonoBehaviour
{
    public float speed = 20f;
    public float damage = 20f;
    private Rigidbody rb;
    private Rogue rogue;

    void Update()
    {
        gameObject.transform.rotation = Quaternion.LookRotation(rb.velocity);
    }
    public void Initialize(Rogue rogueRef)
    {
        rogue = rogueRef;
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
        StartCoroutine(DeactivateAfterTime());
    }

    private IEnumerator DeactivateAfterTime()
    {
        yield return new WaitForSeconds(rogue.arrowLifeTime);
        rogue.ReturnArrowToPool(gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        rogue.ReturnArrowToPool(gameObject);
        if (other.gameObject.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(damage);
        }
    }
}
