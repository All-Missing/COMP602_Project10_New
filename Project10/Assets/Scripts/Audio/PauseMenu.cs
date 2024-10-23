using UnityEngine;
using UnityEngine.SceneManagement;

public enum MenuState { None, PauseMenu, SettingsMenu }
public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    private MenuState currentMenuState = MenuState.None;

    public GameObject pauseMenuUI;
    public GameObject pauseSettingsUI;   // Reference to the settings panel
    public DistanceTracker distanceTracker; // Reference to the DistanceTracker script


    void Update() // Check for key press 'Esc', turns on or off Pause Menu if active or inactive
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        switch (currentMenuState)
        {
            case MenuState.PauseMenu:
                pauseMenuUI.SetActive(true);
                pauseSettingsUI.SetActive(false);
                break;
            case MenuState.SettingsMenu:
                pauseMenuUI.SetActive(false);
                pauseSettingsUI.SetActive(true);
                break;
            case MenuState.None:
                pauseMenuUI.SetActive(false);
                pauseSettingsUI.SetActive(false);
                break;
        }
    }  

    public void SetPauseUIActive()
    {
        currentMenuState = MenuState.PauseMenu;
    }

    public void SetSettingsUIActive()
    {
        currentMenuState = MenuState.SettingsMenu;
    }

    public void CloseAllMenus()
    {
        currentMenuState = MenuState.None;
    }

    public void Resume() // Resumes the game, turning disabling the pause menu, sets timescale to 1 and locks the cursor
    {
        CloseAllMenus();
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        GameIsPaused = false;
    }

    public  void Pause() // Enables the Pause Menu, stops the game and unlocks cursor
    {
        SetPauseUIActive();
        Time.timeScale = 0f;  // Pauses the game

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        distanceTracker.DisplayDistanceOnUI(); // Update Distance Walked

        GameIsPaused = true;
    }
    public void LoadMenu() // Loads the main menu
    {
        Time.timeScale = 1f; // Reset time scale
        SceneManager.LoadScene("Main menu");
    }

    public void QuitGame() // Exits the application
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
