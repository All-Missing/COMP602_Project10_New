using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GrappleHook : MonoBehaviour
{
    public GameObject chestInventoryUI; // The chest inventory UI
    public GameObject interactText;     // Text to show when the player is near
    public GameObject playerItemHUD;    // Grapple HUD that shows when equipped
    public LineRenderer lineRenderer;   // Line for grapple effect
    public Transform cameraTransform;   // Player camera
    public Transform player;            // Player's position
    public float grappleSpeed = 10f;    // Grapple speed
    public float maxGrappleDistance = 50f; // Maximum distance for grapple

    private bool isPlayerNear = false;
    private bool isGrappling = false;   // Whether the player is currently grappling
    private Vector3 grapplePoint;       // Point to grapple to
    private SpringJoint springJoint;    // Spring effect for pulling player

    void Start()
    {
        chestInventoryUI.SetActive(false);
        interactText.SetActive(false);
        playerItemHUD.SetActive(false);  // Start with Grapple HUD hidden
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;
    }

    void Update()
    {
        // Toggle the chest UI if the player is near and presses E
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            ToggleChest();
        }

        // Start grappling if the player presses R and the HUD is active
        if (Input.GetKeyDown(KeyCode.R) && playerItemHUD.activeSelf && !isGrappling)
        {
            StartGrapple();
        }

        // Update the grapple if in progress
        if (isGrappling)
        {
            UpdateGrapple();
        }
    }

    void ToggleChest()
    {
        bool isActive = chestInventoryUI.activeSelf;
        chestInventoryUI.SetActive(!isActive);

        if (isActive)
        {
            interactText.SetActive(false);
        }
        else
        {
            interactText.SetActive(true);
        }
    }

    private void StartGrapple()
    {
        // Raycast from the camera to detect grapple point
        RaycastHit hit;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, maxGrappleDistance))
        {
            grapplePoint = hit.point;
            isGrappling = true;
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, player.position);
            lineRenderer.SetPosition(1, grapplePoint);

            springJoint = player.gameObject.AddComponent<SpringJoint>();
            springJoint.autoConfigureConnectedAnchor = false;
            springJoint.connectedAnchor = grapplePoint;
            springJoint.spring = 10f;
            springJoint.damper = 5f;
            springJoint.maxDistance = Vector3.Distance(player.position, grapplePoint) * 0.8f;
            springJoint.minDistance = 0.1f;
        }
    }

    private void UpdateGrapple()
    {
        // Update line renderer
        lineRenderer.SetPosition(0, player.position);
        lineRenderer.SetPosition(1, grapplePoint);

        // Stop grappling when close enough to the target
        if (Vector3.Distance(player.position, grapplePoint) < 2f)
        {
            StopGrapple();
        }
    }

    private void StopGrapple()
    {
        isGrappling = false;
        lineRenderer.enabled = false;
        Destroy(springJoint);
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

    public void OnGrappleItemClick()
    {
        Debug.Log("Grapple Hook Equipped!");

        // Display the grapple HUD
        playerItemHUD.SetActive(true); // Show Grapple HUD when equipped
    }
}
