using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;

public class SceneQuitTests
{
    [Test]
    public void QuitGame_LogsQuitMessage()
    {
        // Arrange: Create a GameObject and attach the SceneQuit component
        GameObject gameObject = new GameObject();
        SceneQuit sceneQuit = gameObject.AddComponent<SceneQuit>();

        // Act: Call the QuitGame method
        LogAssert.Expect(LogType.Log, "Quit!"); // Expect the debug log output
        sceneQuit.QuitGame();

        // Assert: Check that the correct log was made (LogAssert verifies that the log was created)
        LogAssert.NoUnexpectedReceived(); // Verifies there are no unexpected logs
    }
}
