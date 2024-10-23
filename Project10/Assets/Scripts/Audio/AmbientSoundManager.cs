using UnityEngine;

public class AmbientSoundManager : MonoBehaviour
{
    public AudioSource ambientAudioSource;

    void Start()
    {
        // Call the new method to handle the sound logic
        PlayAmbientSound();
    }

    // New method that handles playing the ambient sound
    public void PlayAmbientSound()
    {
        if (ambientAudioSource != null)
        {
            ambientAudioSource.Play();
        }
    }
}
