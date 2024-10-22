using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UISoundManager : MonoBehaviour
{
    // Use the AudioController's uiAudioSource instead of a separate AudioSource
    public AudioController audioController;  // Reference to AudioController

    public AudioClip hoverSound;    // Hover sound
    public AudioClip clickSound;    // Click sound

    public void PlayHoverSound()
    {
        // Play the hover sound using the AudioController's uiAudioSource
        audioController.uiAudioSource.PlayOneShot(hoverSound);
    }

    public void PlayClickSound()
    {
        // Play the click sound using the AudioController's uiAudioSource
        audioController.uiAudioSource.PlayOneShot(clickSound);
    }
}
