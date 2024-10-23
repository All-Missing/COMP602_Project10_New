using UnityEngine;

public class EndGameTrigger : MonoBehaviour
{
    public GameTimer gameTimer;  // Reference to the GameTimer script

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered by: " + other.gameObject.name);  // Log the object that entered

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player has touched the object!");  // Log when the player touches
            gameTimer.EndGame();  // Trigger the end game event
        }
    }
}
