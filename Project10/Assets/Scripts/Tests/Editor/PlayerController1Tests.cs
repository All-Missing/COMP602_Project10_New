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
        // Set a ground layer
        var groundLayerMask = LayerMask.GetMask("Ground");
        playerController.GetType()
            .GetField("groundLayer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(playerController, groundLayerMask);

        // Call the private GroundCheck() method using reflection
        playerController.GetType()
            .GetMethod("GroundCheck", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(playerController, null);

        // Verify that isGrounded is correctly set
        bool isGrounded = (bool)playerController.GetType()
            .GetField("isGrounded", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(playerController);

        Assert.IsTrue(isGrounded);
    }

    [Test]
    public void PlayerMovement_UpdatesVelocityBasedOnInput()
    {
        // Simulate input values (moving forward)
        Vector3 inputDirection = new Vector3(0, 0, 1); // Forward input

        // Set the desiredMoveDir directly for simplicity
        playerController.GetType()
            .GetField("desiredMoveDir", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(playerController, inputDirection);

        // Simulate the effects of Update() by checking if the velocity was updated correctly
        float moveSpeed = (float)playerController.GetType()
            .GetField("moveSpeed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(playerController);

        Vector3 velocity = (Vector3)playerController.GetType()
            .GetField("velocity", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(playerController);

        Assert.AreEqual(inputDirection.z * moveSpeed, velocity.z);
    }

    [Test]
    public void PlayerShouldRotateTowardsMoveDirection()
    {
        // Simulate movement input in some direction (e.g., right)
        Vector3 moveDirection = new Vector3(1, 0, 0); // Move to the right

        playerController.GetType()
            .GetField("moveDir", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(playerController, moveDirection);

        // Call the method to update movement
        // Rotation based on direction
        Quaternion targetRotation = (Quaternion)playerController.GetType()
            .GetField("targetRotation", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(playerController);

        Assert.AreEqual(Quaternion.LookRotation(moveDirection), targetRotation);
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
        // Simulate input values (e.g., moving forward)
        Vector3 inputDirection = new Vector3(0, 0, 1); // Forward input

        // Set the desiredMoveDir directly for simplicity
        playerController.GetType()
            .GetField("desiredMoveDir", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(playerController, inputDirection);

        // Set isGrounded to true to simulate grounded player
        playerController.GetType()
            .GetField("isGrounded", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(playerController, true);

        // Call Update (indirectly testing movement)
        playerController.GetType()
            .GetMethod("GroundCheck", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(playerController, null);

        // Verify animator received the correct parameters for movement
        float moveAmount = animator.GetFloat("moveAmount");
        Assert.AreEqual(1.0f, moveAmount, 0.1f); // Expecting a value close to 1 for forward movement
    }
}
