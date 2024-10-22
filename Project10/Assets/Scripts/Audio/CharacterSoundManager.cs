using UnityEngine;

using static ParkourController;
public class CharacterSoundManager : MonoBehaviour
{
    public AudioSource audioSource; 
    public AudioClip footstepClip; // The sound clip for the walking sound
    public AudioClip vaultingClip; // The sound clip for the vaulting sound
    public CharacterController controller;   // Reference to the character controller or movement script

private ParkourController parkourController;
    
    void Start()
    {
        // Initialize AudioSource settings
        if (audioSource != null)
        {
            audioSource.loop = false;  // Looping is decided on in specific actions.
        }
    }

    void Update()
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
            audioSource.loop = true;  // Walking sound should loop
            audioSource.Play();
        }
    }

    public void PlayVaultingSound()
{
    // Stop walking sound if it's playing
    if (audioSource.clip == footstepClip && audioSource.isPlaying)
    {
        audioSource.Stop();  // Stop walking sound when vaulting starts
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
