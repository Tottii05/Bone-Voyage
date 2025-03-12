using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Rigidbody rb;
    public float speed = 5f;
    public Camera mainCamera;
    public Animator animator;
    public GameObject bulletPrefab;
    public Transform spawnPoint;

    private bool isAttacking = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
        animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        if (!isAttacking)
        {
            Move();
        }
        ChangeRotation();
        Attack();
    }

    public void Move()
    {
        float moveZ = 0f;
        bool isMovingForward = false;
        bool isMovingBackward = false;

        if (Input.GetKey(KeyCode.W))
        {
            moveZ = 1f;
            isMovingForward = true;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            moveZ = -1f;
            isMovingBackward = true;
        }

        Vector3 movement = new Vector3(0, 0, moveZ).normalized;

        if (movement.magnitude > 0)
        {
            Vector3 moveDirection = transform.forward * speed * Time.deltaTime;
            if (isMovingBackward)
            {
                moveDirection = -transform.forward * speed * Time.deltaTime;
            }
            rb.MovePosition(rb.position + moveDirection);
        }

        animator.SetBool("Run", isMovingForward); 
        animator.SetBool("Backwards", isMovingBackward); 
    }

    public void ChangeRotation()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 lookAtPos = hit.point;
            lookAtPos.y = transform.position.y;
            transform.LookAt(lookAtPos);
        }
    }

    public void Attack()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            StartCoroutine(AttackAfterAnimation());
        }
    }

    private IEnumerator AttackAfterAnimation()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        if (bulletPrefab!= null)
        {
            Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation);
        }
        isAttacking = false;
    }
}
