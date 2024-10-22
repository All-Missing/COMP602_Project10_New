using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BoostChest : MonoBehaviour
{
    public GameObject chestInventoryUI; // Reference to the chest inventory UI
    public GameObject itemIcon; // The item to be shown in the chest
    public GameObject interactText; // Reference to the interact text
    public GameObject playerItemHUD; // HUD showing the equipped item
    public Button addCoinsButton; // Button to add coins
    public TextMeshProUGUI coinText; // Coin display text
    public SpeedBoost speedBoost; // Reference to the SpeedBoost script

    private bool isPlayerNear = false; // Tracks if the player is near the chest

    void Start()
    {
        chestInventoryUI.SetActive(false); // Hide UI at the start
        interactText.SetActive(false); // Hide interact text at the start
        playerItemHUD.SetActive(false); // Hide player HUD initially

        // Set up button listener
        addCoinsButton.onClick.AddListener(OnAddCoinsClick);

        // Initialize coin text
        coinText.text = "Coins: 0";
    }

    void Update()
    {
        // Open chest UI with 'E' when near
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            TryOpenChest();
        }
    }

    void TryOpenChest()
    {
        int currentCoins = GetCurrentCoinCount(); // Get current coins

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
        chestInventoryUI.SetActive(!isActive); // Toggle chest UI visibility

        interactText.SetActive(!isActive); // Toggle interact text
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
            chestInventoryUI.SetActive(false); // Close chest UI

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void OnItemClick()
    {
        // Hide item in chest and show it in HUD
        itemIcon.SetActive(false); // Hide item in the chest
        playerItemHUD.SetActive(true); // Activate the HUD

        Debug.Log("Item Equipped! Speed Boost Unlocked.");
    }

    public void OnAddCoinsClick()
    {
        int currentCoins = GetCurrentCoinCount(); // Get current coins

        currentCoins += 20; // Add 20 coins
        coinText.text = "Coins: " + currentCoins.ToString(); // Update coin display

        addCoinsButton.gameObject.SetActive(false); // Hide button after use
    }

    int GetCurrentCoinCount()
    {
        // Parse coin count from text
        string coinsText = coinText.text.Replace("Coins: ", "").Trim();
        if (int.TryParse(coinsText, out int currentCoins))
        {
            return currentCoins; // Return the current coin count
        }
        else
        {
            Debug.LogError("Invalid coin format: " + coinText.text);
            return 0; // Default to 0 if parsing fails
        }
    }
}