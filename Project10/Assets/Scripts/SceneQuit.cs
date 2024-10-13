using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneQuit : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit!");
    }
}
