using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    [Header("Dash Settings")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1.5f;
    private bool isDashing = false;
    private bool canDash = true;

    private CharacterController controller;
    private Animator animator;
    private Camera mainCamera;
    private Vector3 moveDirection;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (isDashing) return;

        MoveInput();
        RotateTowardsMouse();

        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetMouseButtonDown(1)) && canDash && moveDirection.magnitude > 0)
        {
            StartCoroutine(PerformDash());
        }
    }

    void MoveInput()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector3(moveX, 0f, moveZ).normalized;
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        if (animator != null)
        {
            bool isMoving = moveDirection.magnitude > 0;
            animator.SetBool("isMoving", isMoving);
        }
    }

    IEnumerator PerformDash()
    {
        canDash = false;
        isDashing = true;

        Vector3 dashDir = moveDirection;

        TrailRenderer trail = GetComponent<TrailRenderer>();
        if (trail != null)
        {
            trail.enabled = true;
        }

        controller.enabled = false;

        float elapsedTime = 0f;
        while (elapsedTime < dashDuration)
        {
            transform.position += dashDir * dashSpeed * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        controller.enabled = true;
        isDashing = false;

        if (trail != null)
        {
            trail.enabled = false;
        }

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    void RotateTowardsMouse()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, transform.position);
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 targetPoint = ray.GetPoint(rayDistance);
            Vector3 lookDirection = targetPoint - transform.position;
            lookDirection.y = 0f;

            if (lookDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 15f);
            }
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        
    }

}
