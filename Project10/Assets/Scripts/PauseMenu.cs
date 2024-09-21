using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    void Update()
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

   public void Resume()
{
    pauseMenuUI.SetActive(false);
    Time.timeScale = 1f;
    Cursor.visible = false;
    Cursor.lockState = CursorLockMode.Locked;
    GameIsPaused = false;
}

    void Pause()
{
    pauseMenuUI.SetActive(true);
    Time.timeScale = 0f;  // Pauses the game
    Cursor.visible = true;
    Cursor.lockState = CursorLockMode.None;
    GameIsPaused = true;
}
    public void LoadMenu()
    {
        Time.timeScale = 1f; // Reset time scale
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}