using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    public Slider generalVolumeSlider; 
    public Slider musicVolumeSlider;    
    public Slider characterVolumeSlider;
    public Slider uiVolumeSlider;

    public Text generalVolumeText;      
    public Text musicVolumeText;        
    public Text characterVolumeText;   
    public Text uiVolumeText;

    public AudioSource generalAudioSource;  
    public AudioSource musicAudioSource;    
    public AudioSource characterAudioSource;
    public AudioSource uiAudioSource;

    void Start()
    {
        // Initialize sliders to current audio source volume
        generalVolumeSlider.value = generalAudioSource.volume;
        musicVolumeSlider.value = musicAudioSource.volume;
        characterVolumeSlider.value = characterAudioSource.volume;
        uiVolumeSlider.value = uiAudioSource.volume;

        // Update text with current values
        UpdateGeneralVolume(generalVolumeSlider.value);
        UpdateMusicVolume(musicVolumeSlider.value);
        UpdateCharacterVolume(characterVolumeSlider.value);
        UpdateUiVolume(uiVolumeSlider.value);

        // Add listeners for sliders
        generalVolumeSlider.onValueChanged.AddListener(UpdateGeneralVolume);
        musicVolumeSlider.onValueChanged.AddListener(UpdateMusicVolume);
        characterVolumeSlider.onValueChanged.AddListener(UpdateCharacterVolume);
        uiVolumeSlider.onValueChanged.AddListener(UpdateUiVolume);
    }

    // General volume slider update
    public void UpdateGeneralVolume(float value)
    {
        generalAudioSource.volume = value;
        generalVolumeText.text = Mathf.RoundToInt(value * 100).ToString(); // Show value from 0 to 100
    }

    // Music volume slider update
    public void UpdateMusicVolume(float value)
    {
        musicAudioSource.volume = value;
        musicVolumeText.text = Mathf.RoundToInt(value * 100).ToString(); // Show value from 0 to 100
    }

    // Character sounds volume slider update
    public void UpdateCharacterVolume(float value)
    {
        characterAudioSource.volume = value;
        characterVolumeText.text = Mathf.RoundToInt(value * 100).ToString(); // Show value from 0 to 100
    }

    // Sounds for UI update
    public void UpdateUiVolume(float value)
    {
        uiAudioSource.volume = value;
        uiVolumeText.text = Mathf.RoundToInt(value * 100).ToString(); // Show value from 0 to 100
    }
}
