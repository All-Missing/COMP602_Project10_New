using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Import for Unity's UI components

public class ChestInteraction : MonoBehaviour
{
    public GameObject chestInventoryUI; // Reference to the chest inventory UI
    public GameObject itemIcon; // The item to be shown in the chest (e.g., blue square)
    public GameObject interactText; // Reference to the interact text
    public GameObject playerItemHUD; // Reference to the GameObject for the HUD that shows the item when equipped
    public Button addCoinsButton; // Reference to the button that adds 100 coins
    public float speedMultiplier = 2f; // Speed multiplier for the boost
    private bool isPlayerNear = false;
    private bool speedBoostReady = true; // Check if speed boost can be activated
    private float originalSpeed; // To store player's original speed

    private PlayerController1 playerController; // Reference to the player's movement controller

    void Start()
    {
        chestInventoryUI.SetActive(false); // Hide the inventory UI at the start
        interactText.SetActive(false); // Hide interact text at the start
        playerItemHUD.SetActive(false); // Hide player HUD at the start

        // Ensure the button is set up and listen for its click
        addCoinsButton.onClick.AddListener(OnAddCoinsClick);

        // Find the player movement controller (assuming the player has a PlayerController1 script)
        playerController = FindObjectOfType<PlayerController1>();
        if (playerController != null)
        {
            originalSpeed = playerController.MoveSpeed; // Store the player's original speed
        }
    }

    void Update()
    {
        // If the player is near and presses "E", toggle the chest UI
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            ToggleChest();
        }

        // Check if the player presses "Q" to activate the speed boost
        if (Input.GetKeyDown(KeyCode.Q) && speedBoostReady && playerItemHUD.activeSelf)
        {
            StartCoroutine(ActivateSpeedBoost());
        }
    }

    void ToggleChest()
    {
        bool isActive = chestInventoryUI.activeSelf;
        chestInventoryUI.SetActive(!isActive); // Toggle the chest UI visibility
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            interactText.SetActive(true); // Show the interact text

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            interactText.SetActive(false); // Hide the interact text
            chestInventoryUI.SetActive(false); // Close chest UI if player leaves

            // Lock the cursor back to the game
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void OnItemClick()
    {
        // Logic to equip the item and hide it from the chest inventory
        itemIcon.SetActive(false); // Hide the item in the chest
        Debug.Log("Item Clicked!");

        // Display the item in the player's HUD
        playerItemHUD.SetActive(true); // Activate the HUD GameObject
        // Optionally, you can add logic here to show the item visually on the HUD.
    }

    public void OnAddCoinsClick()
    {
        // Find the coin collection script and add 100 coins
        CollectCoin collectCoinScript = FindObjectOfType<CollectCoin>();
        if (collectCoinScript != null)
        {
            collectCoinScript.AddCoins(100); // Add 100 coins
        }

        // Disable the button after it's clicked to prevent further use
        addCoinsButton.gameObject.SetActive(false); // Make the +100 coins button disappear
    }

    IEnumerator ActivateSpeedBoost()
    {
        if (playerController == null) yield break; // If playerController is missing, exit

        // Increase player speed
        playerController.MoveSpeed *= speedMultiplier;
        speedBoostReady = false;
        Debug.Log("Speed Boost Activated!");

        yield return new WaitForSeconds(5); // Speed boost lasts for 5 seconds

        // Reset player speed to original
        playerController.MoveSpeed = originalSpeed;
        Debug.Log("Speed Boost Ended!");

        yield return new WaitForSeconds(30); // Cooldown duration

        speedBoostReady = true; // Reset cooldown
        Debug.Log("Speed Boost Ready Again!");
    } 
}