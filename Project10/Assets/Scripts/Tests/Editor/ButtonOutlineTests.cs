using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonOutlineTests
{
    private GameObject buttonObject;
    private ButtonOutline buttonOutline;
    private Outline outline;
    private PointerEventData pointerEventData;

    [SetUp]
    public void Setup()
    {
        // Create a new GameObject and add the ButtonOutline component
        buttonObject = new GameObject();
        buttonOutline = buttonObject.AddComponent<ButtonOutline>();

        // Add the Outline component to the button
        outline = buttonObject.AddComponent<Outline>();

        // Simulate the EventSystem and PointerEventData
        pointerEventData = new PointerEventData(EventSystem.current);

        // Initialize the ButtonOutline (manually calling the method from Start)
        buttonOutline.InitializeOutline();
    }

    [Test]
    public void TestOutlineIsDisabledByDefault()
    {
        // Assert: The outline should be disabled by default
        Assert.IsFalse(outline.enabled, "Outline should be disabled by default.");
    }

    [Test]
    public void TestOutlineEnabledOnPointerEnter()
    {
        // Act: Simulate pointer enter event
        buttonOutline.OnPointerEnter(pointerEventData);

        // Assert: The outline should be enabled
        Assert.IsTrue(outline.enabled, "Outline should be enabled when pointer enters.");
    }

    [Test]
    public void TestOutlineDisabledOnPointerExit()
    {
        // Arrange: First simulate pointer enter to enable the outline
        buttonOutline.OnPointerEnter(pointerEventData);
        Assert.IsTrue(outline.enabled, "Outline should be enabled after pointer enters.");

        // Act: Simulate pointer exit event
        buttonOutline.OnPointerExit(pointerEventData);

        // Assert: The outline should be disabled again
        Assert.IsFalse(outline.enabled, "Outline should be disabled when pointer exits.");
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(buttonObject);
    }
}
