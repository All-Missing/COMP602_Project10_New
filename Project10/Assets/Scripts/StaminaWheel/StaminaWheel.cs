using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaWheel : MonoBehaviour
{
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float walkingDrainRate = 10f;  // Drain rate for walking
    [SerializeField] private float sprintingDrainRate = 30f; // Drain rate for sprinting
    [SerializeField] private float staminaRegenRate = 20f; // Rate at which stamina regenerates
    [SerializeField] private Image greenWheel;
    [SerializeField] private Image redWheel;

    private float stamina;
    private bool staminaExhausted = false;
    private bool redWheelRegen = false;

    void Start()
    {
        stamina = maxStamina;
    }

    void Update()
    {
        HandleStamina();  // Handle stamina based on input
        UpdateWheels();   // Update the UI for the stamina wheel
    }

    private void HandleStamina()
    {
        if (Input.GetKey(KeyCode.W)) // Walking
        {
            // Sprinting if LeftShift is also pressed
            if (Input.GetKey(KeyCode.LeftShift) && !staminaExhausted)
            {
                DrainStamina(sprintingDrainRate);
            }
            // Walking (W key only)
            else if (!staminaExhausted)
            {
                DrainStamina(walkingDrainRate);
            }
        }
        else // Regenerate stamina when not walking
        {
            RegenerateStamina();
        }
    }

    private void DrainStamina(float drainRate)
    {
        stamina -= drainRate * Time.deltaTime;
        if (stamina <= 0)
        {
            stamina = 0;
            staminaExhausted = true;
            redWheelRegen = true; // Start regenerating the red wheel
        }
    }

    private void RegenerateStamina()
    {
        if (staminaExhausted)
        {
            redWheel.fillAmount += Time.deltaTime;
            if (redWheel.fillAmount >= 1f)
            {
                redWheelRegen = false;
                staminaExhausted = false; // Allow stamina regeneration
            }
        }
        else if (stamina < maxStamina)
        {
            stamina += staminaRegenRate * Time.deltaTime;
            if (stamina > maxStamina)
            {
                stamina = maxStamina;
            }
        }
    }

    private void UpdateWheels()
    {
        greenWheel.fillAmount = stamina / maxStamina;
        if (!redWheelRegen)
        {
            redWheel.fillAmount = greenWheel.fillAmount; // Sync red wheel with stamina
        }
    }
}