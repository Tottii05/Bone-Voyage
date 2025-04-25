using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Player;

public class WorldMapMovement : MonoBehaviour, IMovementActions
{
    private Player playerActions;
    public Rigidbody rb;
    public Camera mainCamera;
    public Animator animator;

    private Vector3 targetPosition;
    private Vector2 movementInput;
    private bool isMoving = false;
    private float minDistanceToTarget = 0.75f;
    public float moveSpeed = 1.75f;

    private void Awake()
    {
        // Find all objects with WorldMapMovement (i.e., all players in the world map)
        WorldMapMovement[] existingPlayers = FindObjectsOfType<WorldMapMovement>();

        // If there are other players (excluding this one), destroy them
        foreach (WorldMapMovement player in existingPlayers)
        {
            if (player != this) // Don't destroy the current instance
            {
                Destroy(player.gameObject);
            }
        }

        // Now proceed with setup for this player
        playerActions = new Player();
        playerActions.Movement.SetCallbacks(this);
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
        animator = GetComponent<Animator>();
        targetPosition = transform.position;
    }

    private void OnEnable()
    {
        playerActions.Enable();
    }

    private void OnDisable()
    {
        playerActions.Disable();
    }

    private void FixedUpdate()
    {
        UpdateTargetPosition();
        Move();
        ChangeRotation();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
        isMoving = movementInput != Vector2.zero;
    }

    private void UpdateTargetPosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            targetPosition = hit.point;
            targetPosition.y = transform.position.y;
        }
    }

    private void Move()
    {
        if (isMoving)
        {
            float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
            Vector3 directionToTarget = (targetPosition - transform.position).normalized;
            Vector3 moveDirection = Vector3.zero;

            if (distanceToTarget > minDistanceToTarget || movementInput.y != 0 || movementInput.x != 0)
            {
                moveDirection = directionToTarget;
                animator.SetBool("Run", true);
            }
            else
            {
                moveDirection = Vector3.zero;
                rb.velocity = Vector3.zero;
                animator.SetBool("Run", false);
            }

            if (moveDirection != Vector3.zero)
            {
                moveDirection = moveDirection.normalized;
                rb.velocity = moveDirection * moveSpeed;
            }
        }
        else
        {
            rb.velocity = Vector3.zero;
            animator.SetBool("Run", false);
        }
    }

    private void ChangeRotation()
    {
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        if (distanceToTarget > minDistanceToTarget)
        {
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 lookAtPos = hit.point;
                lookAtPos.y = transform.position.y;
                Quaternion targetRotation = Quaternion.LookRotation(lookAtPos - transform.position);
                transform.rotation = targetRotation;
            }
        }
    }
}