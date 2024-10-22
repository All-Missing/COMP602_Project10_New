using UnityEngine;

public class MusicSoundManager : MonoBehaviour
{
    public AudioSource musicSource;    // The AudioSource component that plays the music
    public AudioClip[] musicClips;     // Array to hold your .mp3 tracks
    private int currentTrackIndex = 0; // Keep track of the current music index

    void Start()
    {
        // Start by playing the first track
        PlayMusic();
    }

    void Update()
    {
        // Automatically go to the next track when the current one finishes
        if (!musicSource.isPlaying)
        {
            PlayNextTrack();
        }
    }

    // Play the current music track
    public void PlayMusic()
    {
        if (musicClips.Length > 0)
        {
            musicSource.clip = musicClips[currentTrackIndex];
            musicSource.Play();
        }
    }

    // Play the next track
    public void PlayNextTrack()
    {
        currentTrackIndex = (currentTrackIndex + 1) % musicClips.Length;
        PlayMusic();
    }

    // Play the previous track
    public void PlayPreviousTrack()
    {
        currentTrackIndex = (currentTrackIndex - 1 + musicClips.Length) % musicClips.Length;
        PlayMusic();
    }

    void Awake()
{
    DontDestroyOnLoad(this.gameObject); // Keep this GameObject alive across scenes
} 
}
