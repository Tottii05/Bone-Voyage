using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Player;

public class RockBehaviour : MonoBehaviour, IInteractActions
{
    private Player playerActions;

    public int rockID;
    public RockManager RockManager;
    public Vector3 basePosition;

    public GameObject useLabel;

    //public Canvas canvas;
    private bool playerInTrigger = false;
    private Vector3 playerPosition;

    private void Awake()
    {
        useLabel.SetActive(false);
        playerActions = new Player();
        playerActions.Interact.SetCallbacks(this);
    }

    private void OnEnable()
    {
        playerActions.Enable();
    }

    private void OnDisable()
    {
        playerActions.Disable();
    }

    private void Start()
    {
        RockManager = FindObjectOfType<RockManager>();
        basePosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInTrigger = true;
            playerPosition = other.transform.position;
            useLabel.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerPosition = other.transform.position;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInTrigger = false;
            playerPosition = Vector3.zero;
            useLabel.SetActive(false);
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed && playerInTrigger)
        {
            if (playerPosition != Vector3.zero)
            {
                Vector2 direction = new Vector2(playerPosition.x - transform.position.x, playerPosition.z - transform.position.z);
                direction *= -1;

                float absX = Mathf.Abs(direction.x);
                float absY = Mathf.Abs(direction.y);

                if (absX > absY)
                {
                    direction.y = 0;
                }
                else if (absY > absX)
                {
                    direction.x = 0;
                }
                else
                {
                    direction.x = 0;
                    direction.y = 0;
                }

                direction.Normalize();
                direction.x = Mathf.Round(direction.x);
                direction.y = Mathf.Round(direction.y);
                Vector2Int directionInt = new Vector2Int((int)direction.x, (int)direction.y);

                if (RockManager.TryToMoveRock(this, directionInt))
                {
                    StartCoroutine(MoveRock(new Vector3(
                        transform.position.x + (direction.x * 3),
                        transform.position.y,
                        transform.position.z + (direction.y * 3)
                    )));
                }
            }
        }
    }

    public IEnumerator MoveRock(Vector3 targetPosition)
    {
        float elapsedTime = 0f;
        float duration = 0.5f;
        Vector3 startPosition = transform.position;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }
}
