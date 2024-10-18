using UnityEngine;
using System.Collections;

public class GrappleHook : MonoBehaviour
{
    public float grappleDuration = 3f; // How long the grapple lasts
    public float grappleSpeed = 10f; // Speed at which the player moves to the grapple point
    public LayerMask grappleableLayer; // Layer for grapple targets
    public Transform cameraTransform; // Camera from which to shoot the grapple
    public LineRenderer lineRenderer; // Line Renderer for the hook

    private bool isGrappling;
    private Vector3 grapplePoint;
    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        lineRenderer.enabled = false;
    }

    public void TryGrapple()
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, 20f, grappleableLayer))
        {
            grapplePoint = hit.point;
            isGrappling = true;
            lineRenderer.enabled = true;
            lineRenderer.positionCount = 2;
            StartCoroutine(PerformGrapple());
        }
    }

    IEnumerator PerformGrapple()
    {
        while (isGrappling)
        {
            // Draw line from player to grapple point
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, grapplePoint);

            // Move player toward grapple point
            Vector3 grappleDirection = (grapplePoint - transform.position).normalized;
            characterController.Move(grappleDirection * grappleSpeed * Time.deltaTime);

            // Stop when close enough
            if (Vector3.Distance(transform.position, grapplePoint) < 1f)
            {
                EndGrapple();
            }

            yield return null;
        }
    }

    void EndGrapple()
    {
        isGrappling = false;
        lineRenderer.enabled = false; // Hide the line after the grapple
    }
}

