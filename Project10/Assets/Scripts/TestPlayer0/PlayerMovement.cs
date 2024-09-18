using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;

    private Vector3 moveDirection;
    private Vector3 velocity;

    // Variables for handling gravity
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity;

    [SerializeField] private float jumpHeight;

    public Transform playerRotation;
    public float rotatingSpeed = 90f;

    //References
    private CharacterController controller;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        bool isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);
        float moveZ = Input.GetAxis("Vertical");
        bool isPressingW = Input.GetKey(KeyCode.W);
        bool isPressingLeftShift = Input.GetKey(KeyCode.LeftShift);

        // Handle gravity and ground check
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            animator.SetBool("isJumping", false);  // Ensure jump animation is off when grounded
        }

        moveDirection = new Vector3(0, 0, moveZ);
        moveDirection = transform.TransformDirection(moveDirection);

        if (isGrounded)
        {
            if (!isPressingW && moveDirection == Vector3.zero)
            {
                Idle();
            }
            else if (isPressingW && !isPressingLeftShift)
            {
                Walk();
            }
            else if (isPressingW && isPressingLeftShift)
            {
                Run();
            }

            // Handle jumping
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }

        moveDirection *= moveSpeed;

        // Handle character rotation
        if (Input.GetKey(KeyCode.A))
        {
            playerRotation.Rotate(0, -rotatingSpeed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            playerRotation.Rotate(0, rotatingSpeed * Time.deltaTime, 0);
        }

        // Move the character
        controller.Move(moveDirection * Time.deltaTime);

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void Idle()
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);
        animator.SetBool("isIdle", true);
        moveSpeed = 0;
    }

    private void Walk()
    {
        moveSpeed = walkSpeed;
        animator.SetBool("isWalking", true);
        animator.SetBool("isRunning", false);
    }

    private void Run()
    {
        moveSpeed = runSpeed;
        animator.SetBool("isRunning", true);
    }

    private void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -1.5f * gravity);
        animator.SetBool("isJumping", true);
    }
}
