using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMapScene : MonoBehaviour
{
    // Use OnTriggerEnter instead of OnCollisionEnter for triggers
    public string scenename;
    public List<String> listNames;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Finish")
        {
            LoadNextLevel();
        }
    }

    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("Current Index: "+currentSceneIndex);
        // int nextSceneIndex = currentSceneIndex + 1;

        // Ensure it wraps back to level 1 after the last level
        // if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
        // {
        //     nextSceneIndex = 0;
        // }
        SceneManager.LoadScene(currentSceneIndex+1);
    }
}
