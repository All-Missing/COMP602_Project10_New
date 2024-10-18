using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

[TestFixture]
public class StaminaWheelTests
{
    private StaminaWheel staminaWheel;
    private GameObject testObject;
    private Image greenWheelImage;
    private Image redWheelImage;

    [SetUp]
    public void SetUp()
    {
        // Create a GameObject to attach the StaminaWheel script
        testObject = new GameObject();
        staminaWheel = testObject.AddComponent<StaminaWheel>();

        // Set up images for green and red wheel
        greenWheelImage = new GameObject().AddComponent<Image>();
        redWheelImage = new GameObject().AddComponent<Image>();

        // Use reflection to assign private serialized fields
        typeof(StaminaWheel).GetField("greenWheel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(staminaWheel, greenWheelImage);

        typeof(StaminaWheel).GetField("redWheel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(staminaWheel, redWheelImage);

        // Initialize stamina
        typeof(StaminaWheel).GetField("stamina", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(staminaWheel, 100f);  // Set initial stamina
    }

    [TearDown]
    public void TearDown()
    {
        // Use DestroyImmediate to clean up the test objects
        Object.DestroyImmediate(staminaWheel);
        Object.DestroyImmediate(testObject);
        Object.DestroyImmediate(greenWheelImage.gameObject);
        Object.DestroyImmediate(redWheelImage.gameObject);
    }

    [Test]
    public void StaminaStartsAtMax()
    {
        // Test if stamina starts at maximum value
        float stamina = (float)typeof(StaminaWheel).GetField("stamina", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(staminaWheel);

        Assert.AreEqual(100f, stamina, "Stamina did not start at max value.");
    }

    [Test]
    public void StaminaDrainsWhileSprinting()
    {
        // Simulate sprinting
        staminaWheel.GetType().GetMethod("DrainStamina", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(staminaWheel, new object[] { 30f });

        float stamina = (float)typeof(StaminaWheel).GetField("stamina", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(staminaWheel);

        // Verify stamina has decreased (delta time is not considered here for simplicity)
        Assert.Less(stamina, 100f, "Stamina did not decrease while sprinting.");
    }

    [Test]
    public void StaminaRegeneratesWhenNotMoving()
    {
        // Drain stamina first
        staminaWheel.GetType().GetMethod("DrainStamina", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(staminaWheel, new object[] { 30f });

        // Call the stamina regeneration method
        staminaWheel.GetType().GetMethod("RegenerateStamina", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(staminaWheel, null);

        float stamina = (float)typeof(StaminaWheel).GetField("stamina", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(staminaWheel);

        // Stamina should regenerate, so it should be greater than before
        Assert.Greater(stamina, 70f, "Stamina did not regenerate when not moving.");
    }

   [Test]
public void RedWheelFillsWhenStaminaIsExhausted()
{
    // Set stamina to max before draining
    typeof(StaminaWheel).GetField("stamina", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
        .SetValue(staminaWheel, 100f);

    // Simulate draining stamina over multiple calls
    float drainAmount = 30f; // Use a rate that will definitely bring stamina to zero
    for (int i = 0; i < 5; i++)
    {
        staminaWheel.GetType().GetMethod("DrainStamina", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(staminaWheel, new object[] { drainAmount });
    }

    float stamina = (float)typeof(StaminaWheel).GetField("stamina", 
        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
        .GetValue(staminaWheel);

    // Stamina should be 0, and red wheel should start regenerating
    Assert.AreEqual(0f, stamina, 0.01f, "Stamina did not reach 0 when fully drained.");

    bool redWheelRegen = (bool)typeof(StaminaWheel).GetField("redWheelRegen", 
        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
        .GetValue(staminaWheel);

    Assert.IsTrue(redWheelRegen, "Red wheel regeneration did not start when stamina was exhausted.");
}

    [Test]
    public void GreenWheelUIUpdatesCorrectly()
    {
        // Manually set stamina to half (simulate draining)
        typeof(StaminaWheel).GetField("stamina", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(staminaWheel, 50f); // Setting stamina to 50% of max

        // Directly call the method to update the UI after draining stamina
        staminaWheel.GetType().GetMethod("UpdateWheels", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(staminaWheel, null);

        // Check if the green wheel UI updates accordingly
        float greenWheelFillAmount = greenWheelImage.fillAmount;

        Assert.AreEqual(0.5f, greenWheelFillAmount, "Green wheel UI did not update correctly.");
    }
}
