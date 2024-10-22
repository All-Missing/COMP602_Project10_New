using System.Collections;
using UnityEngine;

public class GrappleHook : MonoBehaviour
{
    public GameObject player; // Reference to the player
    public LineRenderer lineRenderer; // Line for grapple effect
    public float grappleSpeed = 10f; // Speed of movement during grapple
    public float maxGrappleDistance = 50f; // Maximum grapple distance
    public Material grappleMaterial; // Material for line renderer
    public float grappleDuration = 3f; // Duration of grapple (in seconds)

    [SerializeField] private float lineStartWidth = 0.1f; // Line start width
    [SerializeField] private float lineEndWidth = 0.1f; // Line end width
    [SerializeField] private float lineHeightOffset = 1.0f; // Offset to raise the line

    private Vector3 grapplePoint; // Target point of the grapple
    private bool isGrappling = false; // Whether grapple is active
    private CharacterController characterController; // Character controller reference

    void Start()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;
        characterController = player.GetComponent<CharacterController>();

        // Set the material and line widths
        lineRenderer.material = grappleMaterial;
        lineRenderer.startWidth = lineStartWidth;
        lineRenderer.endWidth = lineEndWidth;
    }

    void Update()
    {
        // Start grappling when 'Q' is pressed, if not already grappling
        if (Input.GetKeyDown(KeyCode.R) && !isGrappling)
        {
            StartGrapple();
        }

        // Update the grapple line if the grapple is active
        if (isGrappling)
        {
            UpdateGrappleLine();
        }
    }

    private void StartGrapple()
    {
        RaycastHit hit;
        // Perform a raycast to find a grapple point in front of the player
        if (Physics.Raycast(player.transform.position, player.transform.forward, out hit, maxGrappleDistance))
        {
            grapplePoint = hit.point;
            isGrappling = true;
            lineRenderer.enabled = true;

            // Set the initial line positions with the height offset applied
            lineRenderer.SetPosition(0, player.transform.position + Vector3.up * lineHeightOffset);
            lineRenderer.SetPosition(1, grapplePoint + Vector3.up * lineHeightOffset);

            // Start the grapple movement and duration coroutine
            StartCoroutine(GrappleMovement(grappleDuration));
        }
        else
        {
            Debug.Log("No valid grapple point found.");
        }
    }

    private void UpdateGrappleLine()
    {
        // Continuously update the line with the height offset applied
        lineRenderer.SetPosition(0, player.transform.position + Vector3.up * lineHeightOffset);
        lineRenderer.SetPosition(1, grapplePoint + Vector3.up * lineHeightOffset);
    }

    private IEnumerator GrappleMovement(float duration)
    {
        float elapsedTime = 0f;

        // Move the player toward the grapple point for the specified duration
        while (elapsedTime < duration)
        {
            Vector3 direction = (grapplePoint - player.transform.position).normalized;
            characterController.Move(direction * grappleSpeed * Time.deltaTime);

            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        StopGrapple(); // Stop the grapple after the duration ends
    }

    private void StopGrapple()
    {
        isGrappling = false;
        lineRenderer.enabled = false;

        // Optionally, reset player velocity or state here if needed
        characterController.Move(Vector3.zero); // Ensure no lingering movement

        Debug.Log("Grapple ended.");
    }
}

