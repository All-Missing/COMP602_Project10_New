using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Import for Unity's UI components
using TMPro;

public class TimeStopChestInteract : MonoBehaviour
{
    public GameObject chestInventoryUI; // Reference to the chest inventory UI
    public GameObject itemIcon; // The item to be shown in the chest
    public GameObject interactText; // Reference to the interact text
    public GameObject playerItemHUD; // Reference to the HUD showing the equipped item
    public Button addCoinsButton; // Reference to the button that adds 20 coins
    public TextMeshProUGUI coinText; // Reference to the TextMeshPro component for displaying coins
    public float timeSlowFactor = 0.2f; // Factor to slow down time
    public float timeSlowDuration = 5f; // Duration of the time slow effect

    private bool isPlayerNear = false;
    private bool timeSlowReady = true; // Check if time slow can be activated

    private PlayerController1 playerController; // Reference to the player's movement controller

    void Start()
    {
        chestInventoryUI.SetActive(false); // Hide the inventory UI at the start
        interactText.SetActive(false); // Hide interact text at the start
        playerItemHUD.SetActive(false); // Hide player HUD at the start

        addCoinsButton.onClick.AddListener(OnAddCoinsClick); // Setup button listener
        coinText.text = "Coins: 0"; // Initialize the coin display
    }

    void Update()
    {
        // If the player is near and presses "E", try to open the chest
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            TryOpenChest();
        }

        // Activate time slow when "Q" is pressed and item is equipped
        if (Input.GetKeyDown(KeyCode.Q) && timeSlowReady && playerItemHUD.activeSelf)
        {
            StartCoroutine(ActivateTimeSlow());
        }
    }

    void TryOpenChest()
    {
        int currentCoins = GetCurrentCoinCount();

        if (currentCoins >= 20) // Check if the player has enough coins
        {
            currentCoins -= 20; // Deduct 20 coins
            coinText.text = "Coins: " + currentCoins.ToString(); // Update coin display
            ToggleChest(); // Open the chest UI
        }
        else
        {
            Debug.Log("Not enough coins to open the chest.");
        }
    }

    void ToggleChest()
    {
        bool isActive = chestInventoryUI.activeSelf;
        chestInventoryUI.SetActive(!isActive); // Toggle the chest UI visibility

        interactText.SetActive(!isActive); // Toggle interact text visibility
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            interactText.SetActive(true); // Show interact text

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
            chestInventoryUI.SetActive(false); // Close the chest UI if open

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void OnItemClick()
    {
        itemIcon.SetActive(false); // Hide the item in the chest
        playerItemHUD.SetActive(true); // Show the equipped item in the HUD

        Debug.Log("Item Equipped!");
    }

    public void OnAddCoinsClick()
    {
        int currentCoins = GetCurrentCoinCount();

        currentCoins += 20; // Add 20 coins
        coinText.text = "Coins: " + currentCoins.ToString(); // Update coin display

        addCoinsButton.gameObject.SetActive(false); // Disable button after use
    }

    int GetCurrentCoinCount()
    {
        // Parse the current coin count from the coinText
        string coinsText = coinText.text.Replace("Coins: ", "").Trim();
        if (int.TryParse(coinsText, out int currentCoins))
        {
            return currentCoins;
        }
        else
        {
            Debug.LogError("Invalid coin format: " + coinText.text);
            return 0; // Default to 0 if parsing fails
        }
    }

    IEnumerator ActivateTimeSlow()
    {
        Time.timeScale = timeSlowFactor; // Slow time
        timeSlowReady = false;
        Debug.Log("Time Slow Activated!");

        yield return new WaitForSecondsRealtime(timeSlowDuration); // Wait for the duration

        Time.timeScale = 1f; // Reset time scale
        Debug.Log("Time Slow Ended!");

        yield return new WaitForSeconds(30); // Cooldown duration

        timeSlowReady = true; // Reset cooldown
        Debug.Log("Time Slow Ready Again!");
    }
}