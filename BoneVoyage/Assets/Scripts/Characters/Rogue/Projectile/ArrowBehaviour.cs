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
        gameObject.transform.GetChild(1).transform.Rotate(0, 5, 0);
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

    public void OnTriggerEnter(Collider other)
    {
        if (rogue != null)
        {
            rogue.ReturnArrowToPool(gameObject);
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
                rogue.ReturnArrowToPool(gameObject);
                if (other.gameObject.TryGetComponent(out IDamageable damageable) && other.tag == "Player")
                {
                    damageable.TakeDamage(damage);
                }
            }
        }
    }
}
