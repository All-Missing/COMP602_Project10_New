using UnityEngine;

public class AmbientSoundManager : MonoBehaviour
{
    public AudioSource ambientAudioSource;

    void Start()
    {
        // Play the ambient sound when the game starts
        if (ambientAudioSource != null)
        {
            ambientAudioSource.Play();
        }
    }
}
