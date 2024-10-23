using UnityEngine;
using UnityEngine.SceneManagement;  // This allows us to load scenes

public class TreeTeleport : MonoBehaviour
{
    public string sceneToLoad = "Level 3";  // Assign the name of the next scene

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is the player
        if (other.CompareTag("Player"))
        {
            // Load the next scene (Level 3)
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
