using NUnit.Framework;
using UnityEditor.SceneManagement; // Import for Edit Mode Scene Management
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;

[TestFixture]
public class LevelManagerTests
{
    private LevelManager levelManager;

    [SetUp]
    public void Setup()
    {
        // Create a GameObject for the LevelManager script
        GameObject levelManagerObject = new GameObject("LevelManager");
        levelManager = levelManagerObject.AddComponent<LevelManager>();
        levelManager.sceneName = "Profiles"; // Set the scene name for testing
    }

    [UnityTest]
    public IEnumerator TestSceneChangeInEditMode()
    {
        // Arrange
        var expectedScenePath = "Assets/Scenes/Profiles.unity"; // Replace with the correct path

        // Act & Assert
        Assert.DoesNotThrow(() => 
        {
            EditorSceneManager.OpenScene(expectedScenePath, OpenSceneMode.Single);
        });

        yield return null; // Allow time for the scene to load
    }

    [Test]
    public void TestSceneNameSetting()
    {
        // Arrange
        string expectedSceneName = "TestSceneName";

        // Act
        levelManager.sceneName = expectedSceneName;

        // Assert
        Assert.AreEqual(expectedSceneName, levelManager.sceneName);
    }

    [Test]
    public void TestSceneNameIsNotNullOrEmpty()
    {
        // Arrange
        levelManager.sceneName = "AnotherTestSceneName";

        // Act & Assert
        Assert.IsFalse(string.IsNullOrEmpty(levelManager.sceneName), "Scene name should not be null or empty.");
    }

    [TearDown]
    public void TearDown()
    {
        // Clean up test objects safely
        if (levelManager != null)
        {
            Object.DestroyImmediate(levelManager.gameObject);
        }
    }
}
