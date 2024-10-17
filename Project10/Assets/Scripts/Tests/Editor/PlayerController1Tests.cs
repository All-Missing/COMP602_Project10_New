using NUnit.Framework;
using UnityEngine;

public class PlayerController1Tests
{
    private GameObject playerObject;
    private PlayerController1 playerController;
    private CharacterController characterController;
    private Animator animator;
    private EnvironmentScanner environmentScanner;

    [SetUp]
    public void Setup()
    {
        // Create a temporary GameObject and add the PlayerController1 component
        playerObject = new GameObject();
        playerController = playerObject.AddComponent<PlayerController1>();

        // Add necessary real components (Animator, CharacterController, and EnvironmentScanner)
        characterController = playerObject.AddComponent<CharacterController>();
        animator = playerObject.AddComponent<Animator>();
        environmentScanner = playerObject.AddComponent<EnvironmentScanner>();

        // Assign the components to the PlayerController1 script
        playerController.GetType()
            .GetField("characterController", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(playerController, characterController);

        playerController.GetType()
            .GetField("animator", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(playerController, animator);

        playerController.GetType()
            .GetField("environmentScanner", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(playerController, environmentScanner);
    }

    [TearDown]
    public void Teardown()
    {
        // Clean up the test environment by destroying the GameObject
        Object.DestroyImmediate(playerObject);
    }

    [Test]
    public void PlayerStartsWithControlAndNotInAction()
    {
        // Test that the player starts with control and not in an action state
        Assert.IsTrue(playerController.HasControl);
        Assert.IsFalse(playerController.InAction);
    }

    [Test]
    public void GroundCheck_SetsGroundedStateCorrectly()
    {
        // Arrange: Create a ground object with a collider to simulate ground
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);

        // Assign the "Obstacles" layer to the ground object (ensure it exists in Unity's layer settings)
        int groundLayer = LayerMask.NameToLayer("Obstacles");
        Assert.AreNotEqual(-1, groundLayer, "Obstacles layer is not defined. Make sure to add an 'Obstacles' layer in the project settings.");

        ground.layer = groundLayer;

        // Ensure that the Ground layer exists and is set up correctly
        LayerMask groundLayerMask = LayerMask.GetMask("Obstacles");

        // Set the groundLayer mask on the playerController's groundLayer field using reflection
        playerController.GetType()
            .GetField("groundLayer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(playerController, groundLayerMask);

        // Simulate placing the player just above the ground so they are in contact with it
        playerObject.transform.position = new Vector3(0, 0.1f, 0);  // Close to the plane

        // Act: Call the private GroundCheck() method using reflection
        playerController.GetType()
            .GetMethod("GroundCheck", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(playerController, null);

        // Assert: Check if the player is grounded
        bool isGrounded = (bool)playerController.GetType()
            .GetField("isGrounded", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(playerController);

        Assert.IsTrue(isGrounded, "Player should be grounded.");

        // Clean up the created ground object after the test
        Object.DestroyImmediate(ground);
    }





    [Test]
    public void PlayerMovement_UpdatesVelocityBasedOnInput()
    {
        // Arrange
        Vector3 inputDirection = new Vector3(0, 0, 1); // Simulate forward input

        // Set desiredMoveDir directly via reflection
        playerController.GetType()
            .GetField("desiredMoveDir", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(playerController, inputDirection);

        // Set isGrounded to true to simulate movement on ground
        playerController.GetType()
            .GetField("isGrounded", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(playerController, true);

        // Call the private method to update movement
        playerController.GetType()
            .GetMethod("GroundCheck", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(playerController, null);

        // Assert that the velocity has been updated correctly
        Vector3 velocity = (Vector3)playerController.GetType()
            .GetField("velocity", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(playerController);

        float moveSpeed = (float)playerController.GetType()
            .GetField("moveSpeed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(playerController);

        // Assert that velocity was applied correctly
        Assert.AreEqual(moveSpeed, velocity.z, "Velocity should be updated based on moveSpeed and input direction.");
    }



    [Test]
    public void PlayerShouldRotateTowardsMoveDirection()
    {
        // Arrange: Simulate a rightward movement (positive x-direction)
        Vector3 moveDirection = new Vector3(1, 0, 0); // Move to the right

        // Set the moveDir to simulate the movement direction
        playerController.GetType()
            .GetField("moveDir", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(playerController, moveDirection);

        // Set up the player to allow rotation (grounded and with control)
        playerController.GetType()
            .GetField("hasControl", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(playerController, true);

        // Act: Call Update (this will trigger the rotation logic based on move direction)
        playerController.GetType()
            .GetMethod("GroundCheck", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(playerController, null);

        // Retrieve the targetRotation after the update
        Quaternion targetRotation = (Quaternion)playerController.GetType()
            .GetField("targetRotation", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(playerController);

        // Assert: Ensure the player rotates towards the right (matching the move direction)
        Assert.AreEqual(Quaternion.LookRotation(moveDirection), targetRotation, "Player should rotate towards the move direction.");
    }


    [Test]
    public void LedgeMovement_RestrictsMovementOnLedge()
    {
        // Simulate the player being on a ledge
        var ledgeData = new LedgeData { surfaceHit = new RaycastHit { normal = Vector3.forward } };
        playerController.LedgeData = ledgeData;
        playerController.IsOnLedge = true;

        // Simulate moving towards the ledge
        playerController.GetType()
            .GetField("desiredMoveDir", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(playerController, Vector3.forward);

        // Call the private LedgeMovement() method using reflection
        playerController.GetType()
            .GetMethod("LedgeMovement", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(playerController, null);

        // Assert that the velocity has been set to zero (i.e., movement is restricted)
        Vector3 velocity = (Vector3)playerController.GetType()
            .GetField("velocity", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(playerController);

        Assert.AreEqual(Vector3.zero, velocity);
    }

    [Test]
    public void PlayerShouldMoveAndAnimateCorrectly()
    {
        // Arrange: Load the AnimatorController from Resources (ensure you have one saved in Resources folder)
        RuntimeAnimatorController animatorController = Resources.Load<RuntimeAnimatorController>("TestAnimatorController");
        Assert.NotNull(animatorController, "AnimatorController could not be loaded from Resources.");

        // Assign the loaded controller to the animator
        animator.runtimeAnimatorController = animatorController;

        Vector3 inputDirection = new Vector3(0, 0, 1); // Simulate forward input

        // Set desiredMoveDir
        playerController.GetType()
            .GetField("desiredMoveDir", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(playerController, inputDirection);

        // Set isGrounded to true
        playerController.GetType()
            .GetField("isGrounded", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(playerController, true);

        // Act: Call the private method to simulate movement
        playerController.GetType()
            .GetMethod("GroundCheck", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(playerController, null);

        // Assert: Check if animator's moveAmount parameter was set correctly
        float moveAmount = animator.GetFloat("moveAmount");
        Assert.AreEqual(1.0f, moveAmount, 0.1f); // Expecting a value close to 1 for forward movement
    }



}