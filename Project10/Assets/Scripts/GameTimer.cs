using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    public Text timerText; // Drag your UI Text here in the inspector
    private float timeElapsed;
    private bool isGameStarted;

    void Start()
    {
        // Automatically start the timer when Level 3 is loaded
        if (SceneManager.GetActiveScene().name == "Level3")
        {
            StartGame();
        }
        timeElapsed = 0f;
        UpdateTimerText();
    }

    void Update()
    {
        if (isGameStarted)
        {
            timeElapsed += Time.deltaTime;
            UpdateTimerText();
        }
    }

    // Call this to start the game
    public void StartGame()
    {
        isGameStarted = true;
    }

    // Function to update the timer text on screen
    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(timeElapsed / 60);
        int seconds = Mathf.FloorToInt(timeElapsed % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
