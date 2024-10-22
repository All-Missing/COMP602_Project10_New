using System.Collections;
using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    public float timeScaleBoostFactor = 2f; // Factor to increase time scale
    public float speedBoostDuration = 5f; // Duration of the speed boost
    public float speedBoostCooldown = 30f; // Cooldown duration after speed boost

    private bool speedBoostReady = true; // Check if speed boost can be activated

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            ActivateSpeedBoost();
        }
    }

    public void ActivateSpeedBoost()
    {
        if (speedBoostReady)
        {
            Time.timeScale *= timeScaleBoostFactor; // Increase the timescale for speed effect
            Debug.Log("Speed Boost Activated!");

            StartCoroutine(SpeedBoostCoroutine());
        }
    }

    private IEnumerator SpeedBoostCoroutine()
    {
        speedBoostReady = false; // Mark speed boost as not ready
        yield return new WaitForSeconds(speedBoostDuration); // Wait for duration

        Time.timeScale /= timeScaleBoostFactor; // Reset the timescale
        Debug.Log("Speed Boost Duration Over!");

        yield return new WaitForSeconds(speedBoostCooldown); // Wait for cooldown
        speedBoostReady = true; // Reset cooldown
        Debug.Log("Speed Boost Ready Again!");
    }
}