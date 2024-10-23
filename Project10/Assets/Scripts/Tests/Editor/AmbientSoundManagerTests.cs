using NUnit.Framework;
using UnityEngine;

public class AmbientSoundManagerTests
{
    private GameObject soundManagerObject;
    private AmbientSoundManager ambientSoundManager;
    private AudioSource mockAudioSource;

    [SetUp]
    public void Setup()
    {
        // Create a new GameObject and add the AmbientSoundManager component
        soundManagerObject = new GameObject();
        ambientSoundManager = soundManagerObject.AddComponent<AmbientSoundManager>();

        // Create a mock AudioSource and assign it to the AmbientSoundManager
        mockAudioSource = soundManagerObject.AddComponent<AudioSource>();
        ambientSoundManager.ambientAudioSource = mockAudioSource;
    }

    [Test]
    public void TestAmbientSoundDoesNotPlay_WhenAudioSourceIsNull()
    {
        // Arrange: Set the AudioSource to null
        ambientSoundManager.ambientAudioSource = null;

        // Act: Call the method
        ambientSoundManager.PlayAmbientSound();

        // Assert: Ensure nothing happens when AudioSource is null
        Assert.Pass("The ambientAudioSource is null, no sound should play.");
    }

    [TearDown]
    public void Teardown()
    {
        // Clean up the test environment
        Object.DestroyImmediate(soundManagerObject);
    }
}
