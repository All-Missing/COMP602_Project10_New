using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RainController : MonoBehaviour
{
    // References to the rain particle systems (rain and raindrops)
    public GameObject rainEffect;
    public GameObject rainDropsEffect;

    // Boolean to track if rain is active
    private bool isRaining = false;

    // Update is called once per frame
    void Update()
    {
        // Check if the user presses the "O" key
        if (Input.GetKeyDown(KeyCode.O))
        {
            ToggleRain();
        }
    }

    // Method to toggle the rain on or off
    public void ToggleRain()
    {
        // Toggle the boolean value
        isRaining = !isRaining;

        // Check if rainEffect and rainDropsEffect are not null, then enable/disable
        if (rainEffect != null)
        {
            rainEffect.SetActive(isRaining);
        }

        if (rainDropsEffect != null)
        {
            rainDropsEffect.SetActive(isRaining);
        }

        // Optionally: Add a debug log to see what's happening
        Debug.Log("Rain is " + (isRaining ? "On" : "Off"));
    }
}
