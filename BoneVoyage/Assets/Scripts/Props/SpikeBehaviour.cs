using UnityEngine;
using System.Collections;

public class SpikeBehaviour : MonoBehaviour
{
    private Vector3 initialPos;
    public float speed = 12f;
    public float height = 2f;
    public float holdTime = 0.5f;
    public float returnSpeed = 1f;
    private bool isMoving = false;
    public float baiterTime = 1.2f;
    public float pushForce = 100f;

    void Start()
    {
        initialPos = transform.position;
        SpikeTrigger.OnSpikeTriggerActivated += ActivateSpikes;
    }

    void OnDestroy()
    {
        SpikeTrigger.OnSpikeTriggerActivated -= ActivateSpikes;
    }

    private void ActivateSpikes()
    {
        if (!isMoving)
        {
            StartCoroutine(BaiterTime());
        }
    }

    private IEnumerator MoveSpikes()
    {
        isMoving = true;
        float elapsedTime = 0f;
        Vector3 targetPos = new Vector3(initialPos.x, initialPos.y + height, initialPos.z);

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * speed;
            transform.position = Vector3.Lerp(initialPos, targetPos, elapsedTime);
            yield return null;
        }
        transform.position = targetPos;

        yield return new WaitForSeconds(holdTime);

        elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * returnSpeed;
            transform.position = Vector3.Lerp(targetPos, initialPos, elapsedTime);
            yield return null;
        }
        transform.position = initialPos;

        isMoving = false;
    }

    public IEnumerator BaiterTime()
    {
        yield return new WaitForSeconds(baiterTime);
        StartCoroutine(MoveSpikes());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody playerRb = other.GetComponent<Rigidbody>();
            CharacterBehaviour player = other.GetComponent<CharacterBehaviour>();
            if (playerRb != null)
            {
                Vector3 playerVelocity = playerRb.velocity.normalized;
                Vector3 pushDirection = -playerVelocity;
                if (playerVelocity == Vector3.zero)
                {
                    pushDirection = new Vector3(0, 0, 1); // Default direction if player is stationary
                }
                pushDirection.y = 0; // Ignore vertical component
                playerRb.AddForce(pushDirection.normalized * pushForce, ForceMode.Impulse);
            }
            if (player != null)
            {
                player.FreezeMovement(0.75f);
            }
        }
    }
}