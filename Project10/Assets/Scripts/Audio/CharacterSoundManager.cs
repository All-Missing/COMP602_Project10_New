using UnityEngine;

public class CharacterSoundManager : MonoBehaviour
{
    public AudioSource audioSource; 
    public AudioClip footstepClip;
    public AudioClip vaultingClip;
    public CharacterController controller;

    void Start()
    {
        // Initialize AudioSource settings
        if (audioSource != null)
        {
            audioSource.loop = false;
        }
    }

    void Update()
    {
        CheckCharacterMovementAndPlaySound();
    }

    // Extracted method to check movement and play sounds
    public void CheckCharacterMovementAndPlaySound()
    {
        // Check if the character is moving (walking sound logic)
        if (controller != null && controller.velocity.magnitude > 0.1f && !audioSource.isPlaying)
        {
            PlayFootstepSound();
        }

        // Optional: Add conditions to stop footstep sound if needed
        if (controller != null && controller.velocity.magnitude <= 0.1f && audioSource.clip == footstepClip)
        {
            audioSource.Stop();  // Stop footsteps if the character stops moving
        }
    }

    public void PlayFootstepSound()
    {
        if (audioSource.clip != footstepClip || !audioSource.isPlaying)
        {
            audioSource.clip = footstepClip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void PlayVaultingSound()
    {
        // Stop walking sound if it's playing
        if (audioSource.clip == footstepClip && audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        // Now play vaulting sound
        if (audioSource.clip != vaultingClip || !audioSource.isPlaying)
        {
            audioSource.clip = vaultingClip;
            audioSource.loop = false;
            audioSource.Play();
        }
    }
}
 