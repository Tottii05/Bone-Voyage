using UnityEngine;
using UnityEngine.InputSystem;
using static Player;
using System.Collections;

public class CharacterBehaviour : MonoBehaviour, IMovementActions, ISkillsActions
{
    private Player playerActions;
    public Rigidbody rb;
    public Camera mainCamera;
    public Animator animator;

    public bool isWaiting = true;
    public bool usingSpecial = false;
    private Vector3 targetPosition;
    private Vector2 movementInput;
    private bool isMoving = false;
    private float minDistanceToTarget = 0.75f;

    private ACharacter character;

    private void Awake()
    {
        character = GetComponent<ACharacter>();
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
        animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        targetPosition = transform.position;
        playerActions = new Player();
        playerActions.Movement.SetCallbacks(this);  // Callbacks para movimiento (WASD)
        playerActions.Skills.SetCallbacks(this);   // Callbacks para ataques
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
        if (isWaiting && isMoving)
        {
            float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
            Vector3 directionToTarget = (targetPosition - transform.position).normalized;
            Vector3 moveDirection = Vector3.zero;

            if (distanceToTarget > minDistanceToTarget || movementInput.y != 0 || movementInput.x != 0)
            {
                if (movementInput.y > 0)
                {
                    moveDirection = directionToTarget;
                    animator.SetBool("Run", distanceToTarget > minDistanceToTarget);
                    animator.SetBool("Backwards", false);
                    animator.SetBool("Right", false);
                    animator.SetBool("Left", false);
                }
                else if (movementInput.y < 0)
                {
                    moveDirection = -directionToTarget;
                    animator.SetBool("Run", false);
                    animator.SetBool("Backwards", true);
                    animator.SetBool("Right", false);
                    animator.SetBool("Left", false);
                }

                if (movementInput.x < 0)
                {
                    moveDirection += Vector3.Cross(directionToTarget, Vector3.up).normalized;
                    animator.SetBool("Run", false);
                    animator.SetBool("Backwards", false);
                    animator.SetBool("Right", false);
                    animator.SetBool("Left", true);
                }
                else if (movementInput.x > 0)
                {
                    moveDirection += Vector3.Cross(Vector3.up, directionToTarget).normalized;
                    animator.SetBool("Run", false);
                    animator.SetBool("Backwards", false);
                    animator.SetBool("Right", true);
                    animator.SetBool("Left", false);
                }
                if (distanceToTarget < minDistanceToTarget)
                {
                    moveDirection = Vector3.zero;
                    rb.velocity = Vector3.zero;
                    animator.SetBool("Run", false);
                    animator.SetBool("Backwards", false);
                    animator.SetBool("Right", false);
                    animator.SetBool("Left", false);
                }
            }

            if (moveDirection != Vector3.zero)
            {
                moveDirection = moveDirection.normalized;
                rb.velocity = moveDirection * character.speed;
            }
        }
        else
        {
            rb.velocity = Vector3.zero;
            animator.SetBool("Run", false);
            animator.SetBool("Backwards", false);
            animator.SetBool("Right", false);
            animator.SetBool("Left", false);
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
                if (!usingSpecial)
                {
                    transform.rotation = targetRotation;
                }
            }
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed && isWaiting && !usingSpecial)
        {
            character.Attack();
        }
    }
    public void OnSupport(InputAction.CallbackContext context)
    {
        if (context.performed && isWaiting && !usingSpecial)
        {
            character.Support();
        }
        if (context.canceled && this.gameObject.name == "knight")
        {
            character.Support();
        }
    }
    public void OnSpecial(InputAction.CallbackContext context)
    {
        if (context.performed && isWaiting && !usingSpecial)
        {
            character.Special();
        }
    }
    public void OnConsumable(InputAction.CallbackContext context)
    {
    }
}