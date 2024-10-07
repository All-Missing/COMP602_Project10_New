using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;

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
    }

   public void Resume() // Resumes the game, turning disabling the pause menu, sets timescale to 1 and locks the cursor
{
    pauseMenuUI.SetActive(false);
    Time.timeScale = 1f;
    Cursor.visible = false;
    Cursor.lockState = CursorLockMode.Locked;
    GameIsPaused = false;
}

    void Pause() // Enables the Pause Menu, stops the game and unlocks cursor
{
    pauseMenuUI.SetActive(true);
    Time.timeScale = 0f;  // Pauses the game
    Cursor.visible = true;
    Cursor.lockState = CursorLockMode.None;
    GameIsPaused = true;
}
    public void LoadMenu() // Loads the main menu *not yet connnected
    {
        Time.timeScale = 1f; // Reset time scale
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame() // Exits the application
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
