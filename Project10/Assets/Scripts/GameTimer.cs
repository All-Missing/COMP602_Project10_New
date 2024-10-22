using UnityEngine;
using TMPro;  // For TextMeshPro support
using UnityEngine.UI;  // For handling the UI

public class GameTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;  // Timer display
    public GameObject endGameOverlay;  // Reference to the End Game Canvas (set in Inspector)
    public TextMeshProUGUI endGameMessage;  // Reference to the message text
    private float timeElapsed;
    private bool isGameStarted;

    void Start()
    {
        timeElapsed = 0f;
        StartGame();  // Automatically start the timer
        UpdateTimerText();
        endGameOverlay.SetActive(false);  // Ensure the overlay is hidden at the start
    }

    void Update()
    {
        if (isGameStarted)
        {
            timeElapsed += Time.deltaTime;
            UpdateTimerText();  // Update the timer UI
        }
    }

    public void StartGame()
    {
        isGameStarted = true;
    }

    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(timeElapsed / 60);
        int seconds = Mathf.FloorToInt(timeElapsed % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);  // Format and display the time
    }

    public void EndGame()  // Call this when the player touches the object
    {
        isGameStarted = false;  // Stop the timer
        endGameOverlay.SetActive(true);  // Show the congratulations overlay
        endGameMessage.text = "Congratulations!";  // Update the message
    }
}
