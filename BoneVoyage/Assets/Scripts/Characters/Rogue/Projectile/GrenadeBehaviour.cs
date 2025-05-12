using System.Collections;
using UnityEngine;

public class GrenadeBehaviour : MonoBehaviour
{
    public float force;
    public float damage = 50f;
    private Rigidbody rb;
    private Rogue rogue;
    private GameObject explosionRange;

    public AudioSource explosionSound;
    public AudioClip explosionClip;

    public void Initialize(Rogue rogueRef)
    {
        explosionRange = transform.GetChild(0).gameObject;
        explosionRange.SetActive(false);

        rogue = rogueRef;
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero; 
        rb.angularVelocity = Vector3.zero;
        rb.AddForce(transform.forward * force, ForceMode.Impulse);
        explosionSound.volume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        explosionSound.clip = explosionClip;
        StartCoroutine(ExplodeAfterTime());
    }

    private IEnumerator ExplodeAfterTime()
    {
        yield return new WaitForSeconds(rogue.grenadeLifeTime);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        
        explosionSound.Play();
        gameObject.GetComponent<MeshFilter>().GetComponent<Renderer>().enabled = false;
        explosionRange.SetActive(true);
        yield return new WaitForSeconds(1f);
        explosionRange.SetActive(false);
        gameObject.GetComponent<MeshFilter>().GetComponent<Renderer>().enabled = true;
        rogue.ReturnGrenadeToPool(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(damage);
        }    
    }
}

