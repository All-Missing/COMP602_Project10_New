using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Import for Unity's UI components
using TMPro;

public class ChestInteraction : MonoBehaviour
{
    public GameObject chestInventoryUI; // Reference to the chest inventory UI
    public GameObject itemIcon; // The item to be shown in the chest (e.g., blue square)
    public GameObject interactText; // Reference to the interact text
    public GameObject playerItemHUD; // Reference to the GameObject for the HUD that shows the item when equipped
    public Button addCoinsButton; // Reference to the button that adds 20 coins
    public TextMeshProUGUI coinText; // Reference to the TextMeshPro component for displaying coins
    public float timeSlowFactor = 0.5f; // Factor by which to slow down time (0.5 = half speed)
    public float timeSlowDuration = 5f; // Duration of the time slow effect

    private bool isPlayerNear = false;
    private bool timeSlowReady = true; // Check if time slow can be activated

    private PlayerController1 playerController; // Reference to the player's movement controller

    void Start()
    {
        chestInventoryUI.SetActive(false); // Hide the inventory UI at the start
        interactText.SetActive(false); // Hide interact text at the start
        playerItemHUD.SetActive(false); // Hide player HUD at the start

        // Ensure the button is set up and listen for its click
        addCoinsButton.onClick.AddListener(OnAddCoinsClick);
        
        // Initialize the coin text
        coinText.text = "Coins: 0"; // Ensure initial value is set
    }

    void Update()
    {
        // If the player is near and presses "E", toggle the chest UI
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            ToggleChest();
        }

        // Check if the player presses "Q" to activate the time slow
        if (Input.GetKeyDown(KeyCode.Q) && timeSlowReady && playerItemHUD.activeSelf)
        {
            StartCoroutine(ActivateTimeSlow());
        }
    }

    void ToggleChest()
    {
        bool isActive = chestInventoryUI.activeSelf;
        chestInventoryUI.SetActive(!isActive); // Toggle the chest UI visibility

        // Hide the interact text when the chest UI is open
        if (isActive)
        {
            interactText.SetActive(false);
        }
        else
        {
            interactText.SetActive(true);
        }
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
            interactText.SetActive(false); // Hide interact text
            chestInventoryUI.SetActive(false); // Close chest UI if player leaves

            // Lock the cursor back to the game
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void OnItemClick()
    {
        // Hide the item in the chest and equip it
        itemIcon.SetActive(false); // Hide the item in the chest
        Debug.Log("Item Equipped!");

        // Display the item in the player's HUD
        playerItemHUD.SetActive(true); // Activate the HUD GameObject
    }

    public void OnAddCoinsClick()
    {
        // Attempt to parse the current coin count from coinText
        int currentCoins = 0; // Initialize currentCoins

        // Remove the "Coins: " part and trim any whitespace
        string coinsText = coinText.text.Replace("Coins: ", "").Trim();

        // Check if the remaining text is a valid integer
        if (int.TryParse(coinsText, out currentCoins))
        {
            currentCoins += 20; // Add 20 coins
            coinText.text = "Coins: " + currentCoins.ToString(); // Update the displayed coin count
        }
        else
        {
            Debug.LogError("Coin text is not in a valid format: " + coinText.text);
            // Optionally, you could set a default value if parsing fails
            currentCoins = 0; // Reset to 0 or any default value
            coinText.text = "Coins: " + currentCoins.ToString(); // Update the displayed coin count
        }

        // Disable the button after it's clicked to prevent further use
        addCoinsButton.gameObject.SetActive(false); // Make the +20 coins button disappear
    }

    IEnumerator ActivateTimeSlow()
    {
        // Slow down the time scale
        Time.timeScale = timeSlowFactor;
        timeSlowReady = false;
        Debug.Log("Time Slow Activated!");

        yield return new WaitForSecondsRealtime(timeSlowDuration); // Wait for the duration of the time slow

        // Reset time scale back to normal
        Time.timeScale = 1f;
        Debug.Log("Time Slow Ended!");

        yield return new WaitForSeconds(30); // Cooldown duration

        timeSlowReady = true; // Reset cooldown
        Debug.Log("Time Slow Ready Again!");
    }
}