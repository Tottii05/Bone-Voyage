using System.Collections;
using UnityEngine;

public class ArrowBehaviour : MonoBehaviour
{
    public float speed = 20f;
    public float damage = 20f;
    private Rigidbody rb;
    private Rogue rogue;

    public void Initialize(Rogue rogueRef)
    {
        rogue = rogueRef;
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
        Debug.Log("Arrow initialized");
        StartCoroutine(DeactivateAfterTime());
    }

    private IEnumerator DeactivateAfterTime()
    {
        yield return new WaitForSeconds(rogue.arrowLifeTime);
        rogue.ReturnArrowToPool(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        rogue.ReturnArrowToPool(gameObject);
    }
}
