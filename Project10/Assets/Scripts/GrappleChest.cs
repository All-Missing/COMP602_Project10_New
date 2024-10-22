using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GrappleChest : MonoBehaviour
{
    public GameObject chestInventoryUI; // Reference to the chest inventory UI
    public GameObject itemIcon; // The item to be shown in the chest
    public GameObject interactText; // Reference to the interact text
    public GameObject playerItemHUD; // HUD showing the equipped item
    public Button addCoinsButton; // Button to add coins
    public TextMeshProUGUI coinText; // Coin display text
    public float timeSlowFactor = 5f; // Factor to slow time
    public float timeSlowDuration = 5f; // Duration of time slow

    private bool isPlayerNear = false; // Tracks if the player is near the chest
    private bool timeSlowReady = true; // Check if time slow can be activated

    // Reference to GrappleHook script to enable its ability
    public GrappleHook grappleHook; 

    private PlayerController1 playerController; // Reference to the player's controller

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
            ToggleChest();
        }

        // Activate time slow when 'Q' is pressed and item is equipped
        if (Input.GetKeyDown(KeyCode.Q) && timeSlowReady && playerItemHUD.activeSelf)
        {
            StartCoroutine(ActivateTimeSlow());
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

        Debug.Log("Item Equipped! Grapple Hook Unlocked.");

        // Enable the grapple hook ability
        grappleHook.enabled = true; // Activates the grapple hook script
    }

    public void OnAddCoinsClick()
    {
        int currentCoins = 0;

        // Parse coin count from text
        string coinsText = coinText.text.Replace("Coins: ", "").Trim();
        if (int.TryParse(coinsText, out currentCoins))
        {
            currentCoins += 20;
            coinText.text = "Coins: " + currentCoins.ToString();
        }
        else
        {
            Debug.LogError("Invalid coin format: " + coinText.text);
            currentCoins = 0;
            coinText.text = "Coins: 0";
        }

        addCoinsButton.gameObject.SetActive(false); // Hide button after use
    }

    IEnumerator ActivateTimeSlow()
    {
        Time.timeScale = timeSlowFactor;
        timeSlowReady = false;
        Debug.Log("Time Slow Activated!");

        yield return new WaitForSecondsRealtime(timeSlowDuration);

        Time.timeScale = 1f;
        Debug.Log("Time Slow Ended!");

        yield return new WaitForSeconds(30); // Cooldown duration

        timeSlowReady = true;
        Debug.Log("Time Slow Ready Again!");
    }
}
