using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableTree : MonoBehaviour, IDamageable
{
    public GameObject treeMesh;
    public float health = 50f;
    private Vector3 lastHitDirection;
    public float fallSpeed = 90f;
    public float maxFallAngle = 80f;
    private bool isFalling = false;
    private float currentAngle = 0f;
    private Vector3 fallAxis;
    private bool isVibrating = false;

    public void GetHit(GameObject other, float damage)
    {
        TakeDamage(damage);
        if (!isVibrating)
        {
            StartCoroutine(HitEffect(0.2f, 0.1f));
        }
        if (health <= 0 && !isFalling)
        {
            lastHitDirection = (other.transform.position - transform.position).normalized;
            StartFalling();
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }

    private void StartFalling()
    {
        lastHitDirection = new Vector3(lastHitDirection.x, 0, lastHitDirection.z).normalized;

        Vector3 fallDirection = -lastHitDirection;

        fallAxis = Vector3.Cross(Vector3.up, fallDirection);

        isFalling = true;
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    private IEnumerator HitEffect(float duration, float intensity)
    {
        if (treeMesh == null) yield break;

        isVibrating = true;
        Vector3 originalPosition = treeMesh.transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            Vector3 randomOffset = Random.insideUnitSphere * intensity;
            treeMesh.transform.localPosition = originalPosition + new Vector3(randomOffset.x, randomOffset.y * 0.2f, randomOffset.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        treeMesh.transform.localPosition = originalPosition;
        isVibrating = false;
    }

    void Update()
    {
        if (isFalling)
        {
            float angleThisFrame = fallSpeed * Time.deltaTime;

            if (currentAngle + angleThisFrame > maxFallAngle)
            {
                angleThisFrame = maxFallAngle - currentAngle;
                isFalling = false;
                Die();
            }

            transform.RotateAround(transform.position, fallAxis, angleThisFrame);
            currentAngle += angleThisFrame;
        }
    }
}