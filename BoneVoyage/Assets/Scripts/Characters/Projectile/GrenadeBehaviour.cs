using System.Collections;
using UnityEngine;

public class GrenadeBehaviour : MonoBehaviour
{
    public float force;
    public float damage = 50f;
    private Rigidbody rb;
    private Rogue rogue;
    private GameObject explosionRange;

    public void Initialize(Rogue rogueRef)
    {
        explosionRange = transform.GetChild(0).gameObject;
        explosionRange.SetActive(false);

        rogue = rogueRef;
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero; 
        rb.angularVelocity = Vector3.zero;
        rb.AddForce(transform.forward * force, ForceMode.Impulse);
        Debug.Log("Grenade initialized");
        StartCoroutine(ExplodeAfterTime());
    }

    private IEnumerator ExplodeAfterTime()
    {
        yield return new WaitForSeconds(rogue.grenadeLifeTime);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        gameObject.GetComponent<MeshFilter>().GetComponent<Renderer>().enabled = false;
        explosionRange.SetActive(true);
        yield return new WaitForSeconds(1f);
        explosionRange.SetActive(false);
        gameObject.GetComponent<MeshFilter>().GetComponent<Renderer>().enabled = true;
        rogue.ReturnGrenadeToPool(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        //metodo de hacer daño al enemigo
        /*
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
        }
        rogue.ReturnGrenadeToPool(gameObject);
        */
    }
}

