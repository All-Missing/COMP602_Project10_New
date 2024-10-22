using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    public Slider generalVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider characterVolumeSlider;
    public Slider uiVolumeSlider;
    public Slider ambientVolumeSlider;

    public Text generalVolumeText;
    public Text musicVolumeText;
    public Text characterVolumeText;
    public Text uiVolumeText;
    public Text ambientVolumeText;

    public AudioSource generalAudioSource;
    public AudioSource musicAudioSource;
    public AudioSource characterAudioSource;
    public AudioSource uiAudioSource;
    public AudioSource ambientAudioSource;



    void Start()
    {
        // Initialize sliders to current audio source volume
        generalVolumeSlider.value = 1f;
        musicVolumeSlider.value = musicAudioSource.volume;
        characterVolumeSlider.value = characterAudioSource.volume;
        uiVolumeSlider.value = uiAudioSource.volume;
        ambientVolumeSlider.value = ambientAudioSource.volume;

        UpdateMusicVolume(musicVolumeSlider.value);
        UpdateCharacterVolume(characterVolumeSlider.value);
        UpdateUiVolume(uiVolumeSlider.value);
        UpdateAmbientVolume(ambientVolumeSlider.value);

        // Add listeners for sliders
        generalVolumeSlider.onValueChanged.AddListener(UpdateGeneralVolume);
        musicVolumeSlider.onValueChanged.AddListener(UpdateMusicVolume);
        characterVolumeSlider.onValueChanged.AddListener(UpdateCharacterVolume);
        uiVolumeSlider.onValueChanged.AddListener(UpdateUiVolume);
        ambientVolumeSlider.onValueChanged.AddListener(UpdateAmbientVolume);
    }

    // General volume slider update
    public void UpdateGeneralVolume(float generalVolume)
    {
        // Multiply each individual volume slider value by the general volume
        musicAudioSource.volume = musicVolumeSlider.value * generalVolume;
        characterAudioSource.volume = characterVolumeSlider.value * generalVolume;
        uiAudioSource.volume = uiVolumeSlider.value * generalVolume;
        ambientAudioSource.volume = ambientVolumeSlider.value * generalVolume;

        // Update general volume text
        generalVolumeText.text = Mathf.RoundToInt(generalVolume * 100).ToString();
    }
    // Music volume slider update
    public void UpdateMusicVolume(float value)
    {
        musicAudioSource.volume = value;
        musicVolumeText.text = Mathf.RoundToInt(value * 100).ToString(); // Show value from 0 to 100
        UpdateGeneralVolume(generalVolumeSlider.value);
    }

    // Character sounds volume slider update
    public void UpdateCharacterVolume(float value)
    {
        characterAudioSource.volume = value;
        characterVolumeText.text = Mathf.RoundToInt(value * 100).ToString(); // Show value from 0 to 100
        UpdateGeneralVolume(generalVolumeSlider.value);
    }

    // Sounds for UI update
    public void UpdateUiVolume(float value)
    {
        uiAudioSource.volume = value;
        uiVolumeText.text = Mathf.RoundToInt(value * 100).ToString(); // Show value from 0 to 100
        Debug.Log("UI Volume: " + uiAudioSource.volume); // Log the updated volume
        UpdateGeneralVolume(generalVolumeSlider.value);
    }

    // General World Audio update
    public void UpdateAmbientVolume(float value)
    {
        ambientAudioSource.volume = value;
        ambientVolumeText.text = Mathf.RoundToInt(value * 100).ToString(); // Show value from 0 to 100
        UpdateGeneralVolume(generalVolumeSlider.value);
    }
}
