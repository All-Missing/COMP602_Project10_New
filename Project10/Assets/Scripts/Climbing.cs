using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using StarterAssets;
public class Climbing : MonoBehaviour
{
    public float climbingSpeed = 4f;            // Speed for climbing movement
    public LayerMask climbableLayer;            // Layer for climbable surfaces
    public float wallDistance = 0.5f;           // Distance to detect climbable surfaces

    private ThirdPersonController controller;   // Reference to ThirdPersonController
    private bool isClimbing = false;            // Internal climbing state
    private CharacterController charController; // Reference to the CharacterController

    void Start()
    {
        controller = GetComponent<ThirdPersonController>();
        charController = GetComponent<CharacterController>();

        if (controller == null || charController == null)
        {
            Debug.LogError("ThirdPersonController or CharacterController is missing.");
        }
    }

    void Update()
    {
        // Check if the player is near a climbable wall and pressing the 'E' key
        if (Input.GetKey(KeyCode.E) && IsNearClimbableWall())
        {
            StartClimbing();
        }
        else if (!IsNearClimbableWall() || Input.GetKeyUp(KeyCode.E))
        {
            StopClimbing();
        }

        if (isClimbing)
        {
            ClimbMovement();
        }
    }

    void StartClimbing()
    {
        isClimbing = true;
        controller.isClimbing = true;           // Inform the ThirdPersonController that the player is climbing
        controller.SetVerticalVelocity(0);      // Stop vertical velocity (gravity)
    }

    void StopClimbing()
    {
        isClimbing = false;
        controller.isClimbing = false;          // Reset climbing flag in the ThirdPersonController
    }

    void ClimbMovement()
    {
        float verticalInput = Input.GetAxis("Vertical");     // Get input for climbing up and down
        float horizontalInput = Input.GetAxis("Horizontal"); // Get input for climbing sideways

        Vector3 climbDirection = new Vector3(horizontalInput, verticalInput, 0).normalized;
        charController.Move(climbDirection * climbingSpeed * Time.deltaTime);  // Move the player while climbing
    }

    bool IsNearClimbableWall()
    {
        // Cast a ray forward from the player's position to detect climbable surfaces
        return Physics.Raycast(transform.position, transform.forward, wallDistance, climbableLayer);
    }
}

