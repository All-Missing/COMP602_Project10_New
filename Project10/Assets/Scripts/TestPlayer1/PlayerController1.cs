using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using UnityEngine;

public class PlayerController1 : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotationSpeed = 500f;

    [Header("Ground Check Setting")]
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] Vector3 groundCheckOffSet;
    [SerializeField] LayerMask groundLayer;

    bool isGrounded;
    bool hasControl = true;

    Vector3 desireMoveDir;
    Vector3 moveDir;
    Vector3 velocity;



    public bool IsOnLedge { get; set; }

    public LedgeData LedgeData { get; set; }

    float ySpeed;
    Quaternion targetRotation;

    CameraController cameraController;
    Animator animator;
    CharacterController characterController;
    EnvironmentScanner environmentScanner; //Need environmentScanner object to detect ledge height

    private void Awake()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();        
        environmentScanner = GetComponent<EnvironmentScanner>();
    }

    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        float moveAmount = Mathf.Clamp01(Mathf.Abs(h) + Mathf.Abs(v));

        var moveInput = (new Vector3(h, 0, v)).normalized;

        desireMoveDir = cameraController.PlanarRotation * moveInput;
        moveDir = desireMoveDir;

        if (!hasControl)
            return;

        velocity = Vector3.zero;

        animator.SetBool("isGrounded", isGrounded); //Trigger to perform animation when grounded
        GroundCheck();        
        if (isGrounded)
        {
            ySpeed = -0.5f;
            velocity = desireMoveDir * moveSpeed;

            IsOnLedge = environmentScanner.LedgeCheck(desireMoveDir, out LedgeData ledgeData);
            if (IsOnLedge)
            {                
                LedgeData = ledgeData;
                LedgeMovement();
                //Debug.Log("On Ledge");
            }

        }
        else
        {
            ySpeed+= Physics.gravity.y * Time.deltaTime;

            velocity = transform.forward * moveSpeed / 2;
        }

        
        velocity.y = ySpeed;

        characterController.Move(velocity * Time.deltaTime);

        if (moveAmount > 0)
        {                
            // transform.rotation = Quaternion.LookRotation(desireMoveDir);
            targetRotation = Quaternion.LookRotation(moveDir);
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation,
            targetRotation, rotationSpeed * Time.deltaTime);

        animator.SetFloat("moveAmount", moveAmount, 0.2f, Time.deltaTime);            
    }

    //This method to handle character ground check.
    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(transform.TransformPoint(groundCheckOffSet), groundCheckRadius, groundLayer);
    }

    //This method to restrict character's movement when character position stay close to the ledge object
    void LedgeMovement()
    {
        float angle = Vector3.Angle(LedgeData.surfaceHit.normal, desireMoveDir);

        if (angle < 90)
        {
            velocity = Vector3.zero;
            moveDir = Vector3.zero;
        }
    }

    public void SetControl(bool hasControl)
    {
        this.hasControl = hasControl;
        characterController.enabled = hasControl;

        if (!hasControl)
        {
            animator.SetFloat("moveAround", 0f);
            targetRotation = transform.rotation;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawSphere(transform.TransformPoint(groundCheckOffSet), groundCheckRadius);    
    }

    public float RotationSpeed => rotationSpeed;
}
