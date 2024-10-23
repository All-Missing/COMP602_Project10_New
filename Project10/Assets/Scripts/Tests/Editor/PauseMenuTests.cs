using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class PauseMenuTests
{
    private GameObject pauseMenuGameObject;
    private PauseMenu pauseMenu;
    private GameObject pauseMenuUI;
    private GameObject pauseSettingsUI;
    private DistanceTracker mockDistanceTracker;

    [SetUp]
    public void Setup()
    {
        // Create a new GameObject to attach the PauseMenu component to
        pauseMenuGameObject = new GameObject();
        pauseMenu = pauseMenuGameObject.AddComponent<PauseMenu>();

        // Mock GameObjects for pauseMenuUI and pauseSettingsUI
        pauseMenuUI = new GameObject();
        pauseMenu.pauseMenuUI = pauseMenuUI;

        pauseSettingsUI = new GameObject();
        pauseMenu.pauseSettingsUI = pauseSettingsUI;

        // Mock the DistanceTracker
        mockDistanceTracker = pauseMenuGameObject.AddComponent<DistanceTracker>();
        pauseMenu.distanceTracker = mockDistanceTracker;

        // Initially deactivate UI elements
        pauseMenuUI.SetActive(false);
        pauseSettingsUI.SetActive(false);
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(pauseMenuGameObject);
    }

    [Test]
    public void TestResume_DisablesPauseMenuAndRestoresTime()
    {
        // Arrange
        pauseMenu.Pause(); // First pause the game to simulate pausing
        
        // Act
        pauseMenu.Resume();
        
        // Assert
        Assert.IsFalse(PauseMenu.GameIsPaused, "Game should not be paused");
        Assert.AreEqual(1f, Time.timeScale, "Time scale should be restored to 1 when resumed");
        Assert.IsFalse(pauseMenuUI.activeSelf, "Pause menu UI should be inactive");
        Assert.IsFalse(pauseSettingsUI.activeSelf, "Settings menu UI should be inactive");
        Assert.IsFalse(Cursor.visible, "Cursor should be hidden when resumed");
        Assert.AreEqual(CursorLockMode.Locked, Cursor.lockState, "Cursor should be locked when resumed");
    }

    [Test]
    public void TestCloseAllMenus_DisablesAllUI()
    {
        // Act
        pauseMenu.CloseAllMenus();
        
        // Assert
        Assert.AreEqual(MenuState.None, GetCurrentMenuState(pauseMenu), "No menu should be active");
        Assert.IsFalse(pauseMenuUI.activeSelf, "Pause menu UI should be inactive");
        Assert.IsFalse(pauseSettingsUI.activeSelf, "Settings UI should be inactive");
    }

    [Test]
    public void TestQuitGame_CallsApplicationQuit()
    {
        // Act & Assert
        LogAssert.Expect(LogType.Log, "Quitting game...");
        pauseMenu.QuitGame();
    }

    // Helper method to get the private currentMenuState field
    private MenuState GetCurrentMenuState(PauseMenu menu)
    {
        var fieldInfo = typeof(PauseMenu).GetField("currentMenuState", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        return (MenuState)fieldInfo.GetValue(menu);
    }
}
