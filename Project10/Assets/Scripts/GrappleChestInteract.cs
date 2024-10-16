using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GrappleChestInteract : MonoBehaviour
{
    public GameObject chestInventoryUI; // Reference to the chest inventory UI
    public GameObject itemIcon; // The item to be shown in the chest (grappling hook icon)
    public GameObject interactText; // Reference to the interact text
    public GameObject playerItemHUD; // HUD for equipped item (grappling hook)
    public Button addCoinsButton; // Button for coins (can be used or hidden for the grapple hook)
    public Button equipGrappleButton; // Button to equip the grappling hook
    public float grappleCooldown = 10f; // Cooldown for grappling hook

    private bool isPlayerNear = false;
    private bool grappleReady = true; // Whether the grappling hook is ready to use
    private PlayerController playerController;
    private GrappleHook grappleHook; // Reference to the GrappleHook script

    void Start()
    {
        chestInventoryUI.SetActive(false);
        interactText.SetActive(false);
        playerItemHUD.SetActive(false);

        // Ensure buttons are set up and listen for their clicks
        equipGrappleButton.onClick.AddListener(OnEquipGrappleClick);

        // Find the player and the grappling hook script
        playerController = FindObjectOfType<PlayerController>();
        grappleHook = FindObjectOfType<GrappleHook>();
    }

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            ToggleChest();
        }

        if (Input.GetKeyDown(KeyCode.R) && grappleReady && grappleHook != null)
        {
            StartCoroutine(ActivateGrappleHook());
        }
    }

    void ToggleChest()
    {
        bool isActive = chestInventoryUI.activeSelf;
        chestInventoryUI.SetActive(!isActive);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            interactText.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            interactText.SetActive(false);
            chestInventoryUI.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void OnEquipGrappleClick()
    {
        // Equip the grappling hook and show it on the HUD
        itemIcon.SetActive(false); // Hide item in chest
        playerItemHUD.SetActive(true); // Show grappling hook in player's HUD

        Debug.Log("Grappling Hook Equipped!");
    }

    IEnumerator ActivateGrappleHook()
    {
        grappleHook.TryGrapple(); // Trigger the grappling hook
        grappleReady = false;
        Debug.Log("Grappling Hook Activated!");

        // Optionally, you could add a visual cooldown timer here
        yield return new WaitForSeconds(grappleHook.grappleDuration); // Wait for grappling hook to finish

        yield return new WaitForSeconds(grappleCooldown); // Cooldown time
        grappleReady = true;
        Debug.Log("Grappling Hook Ready Again!");
    }
}

