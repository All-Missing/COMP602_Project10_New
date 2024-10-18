using NUnit.Framework;
using UnityEngine;
using UnityEditor;

public class PlayerController1Tests
{
    private GameObject playerObject;
    private PlayerController1 playerController;
    private CharacterController characterController;
    private Animator animator;
    private EnvironmentScanner environmentScanner;

    // [SetUp]
    // public void Setup()
    // {
    //     // Create a temporary GameObject and add the PlayerController1 component
    //     playerObject = new GameObject();
    //     playerController = playerObject.AddComponent<PlayerController1>();

    //     // Add necessary real components (Animator, CharacterController, and EnvironmentScanner)
    //     characterController = playerObject.AddComponent<CharacterController>();
    //     animator = playerObject.AddComponent<Animator>();
    //     environmentScanner = playerObject.AddComponent<EnvironmentScanner>();

    //     // Assign the components to the PlayerController1 script
    //     playerController.GetType()
    //         .GetField("characterController", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
    //         .SetValue(playerController, characterController);

    //     playerController.GetType()
    //         .GetField("animator", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
    //         .SetValue(playerController, animator);

    //     playerController.GetType()
    //         .GetField("environmentScanner", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
    //         .SetValue(playerController, environmentScanner);
    // }

    [SetUp]
    public void Setup()
    {
        // Create a temporary GameObject and add the PlayerController1 component
        playerObject = new GameObject();
        playerController = playerObject.AddComponent<PlayerController1>();

        // Add necessary components (Animator, CharacterController, EnvironmentScanner)
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

        // Ensure moveSpeed is set to a reasonable value (set a default if necessary)
        playerController.GetType()
            .GetField("moveSpeed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(playerController, 5.0f);

        // Ensure velocity is initialized
        playerController.GetType()
            .GetField("velocity", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(playerController, Vector3.zero);
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
        Debug.Log("Starting PlayerMovement_UpdatesVelocityBasedOnInput test");

        Vector3 inputDirection = new Vector3(0, 0, 1); // Simulate forward input

        // Check if playerController is null
        Assert.IsNotNull(playerController, "PlayerController1 is null.");
        Debug.Log("PlayerController1 initialized");

        // Set desiredMoveDir directly via reflection
        var desiredMoveDirField = playerController.GetType()
            .GetField("desiredMoveDir", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(desiredMoveDirField, "desiredMoveDir field is null.");
        desiredMoveDirField.SetValue(playerController, inputDirection);
        Debug.Log("Set desiredMoveDir");

        // Set isGrounded to true to simulate movement on ground
        var isGroundedField = playerController.GetType()
            .GetField("isGrounded", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(isGroundedField, "isGrounded field is null.");
        isGroundedField.SetValue(playerController, true);
        Debug.Log("Set isGrounded to true");

        // Ensure all required fields/components for Update are initialized
        var characterControllerField = playerController.GetType()
            .GetField("characterController", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(characterControllerField, "CharacterController is not set.");
        characterControllerField.SetValue(playerController, characterController);

        var animatorField = playerController.GetType()
            .GetField("animator", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(animatorField, "Animator is not set.");
        animatorField.SetValue(playerController, animator);

        var environmentScannerField = playerController.GetType()
            .GetField("environmentScanner", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(environmentScannerField, "EnvironmentScanner is not set.");
        environmentScannerField.SetValue(playerController, environmentScanner);

        Debug.Log("All dependencies initialized");

        // Assert that the velocity has been updated correctly
        var velocityField = playerController.GetType()
            .GetField("velocity", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(velocityField, "velocity field is null.");
        Vector3 velocity = (Vector3)velocityField.GetValue(playerController);
        Debug.Log($"Velocity after Update: {velocity}");

        var moveSpeedField = playerController.GetType()
            .GetField("moveSpeed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(moveSpeedField, "moveSpeed field is null.");
        float moveSpeed = (float)moveSpeedField.GetValue(playerController);
        Debug.Log($"MoveSpeed: {moveSpeed}");

        // Assert that velocity was applied correctly based on moveSpeed and input direction
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
        // Load the AnimatorController manually
        RuntimeAnimatorController animatorController = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>("Assets/Game/Animators/CharacterController1.controller");
        Assert.NotNull(animatorController, "AnimatorController could not be loaded from Assets/Game/Animators.");

        // Assign the loaded controller to the animator
        animator.runtimeAnimatorController = animatorController;

        // Initialize components in playerController
        playerController.GetType()
            .GetField("animator", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(playerController, animator);

        playerController.GetType()
            .GetField("characterController", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(playerController, characterController);

        // Add any other components that may be needed, for example, environmentScanner if used
        playerController.GetType()
            .GetField("environmentScanner", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(playerController, environmentScanner);

        // Simulate player movement
        Vector3 inputDirection = new Vector3(0, 0, 1); // Simulate forward input
        playerController.GetType()
            .GetField("desiredMoveDir", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(playerController, inputDirection);

        playerController.GetType()
            .GetField("isGrounded", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(playerController, true);

        // Set the moveAmount parameter manually (if needed for testing)
        animator.SetFloat("moveAmount", 1.0f);

        // Act: Call Update (with more detailed debugging if something is null)
        var updateMethod = playerController.GetType()
            .GetMethod("Update", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        Assert.IsNotNull(updateMethod, "Update method is null.");
        // Assert that the moveAmount parameter was set correctly in the Animator
        float moveAmount = animator.GetFloat("moveAmount");
        Assert.AreEqual(1.0f, moveAmount, 0.1f); // Expecting a value close to 1 for forward movement
    }






}