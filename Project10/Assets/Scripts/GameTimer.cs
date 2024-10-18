using UnityEngine;
using TMPro;  // Make sure to include the TMPro namespace for TextMeshPro support

public class GameTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;  // Change Text to TextMeshProUGUI
    private float timeElapsed;
    private bool isGameStarted;

    void Start()
    {
        timeElapsed = 0f;
        StartGame();  // Automatically start the timer when Level 3 is loaded
        UpdateTimerText();
    }

    void Update()
    {
        if (isGameStarted)
        {
            timeElapsed += Time.deltaTime;
            UpdateTimerText();  // Update the UI with the time
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
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);  // Update TextMeshPro text
    }
}
