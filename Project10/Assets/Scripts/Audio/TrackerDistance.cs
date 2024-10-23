using UnityEngine;
using UnityEngine.UI;

public class DistanceTracker : MonoBehaviour
{
    private Vector3 lastPosition;
    private float totalDistance = 0f;

    public Text distanceText; // Reference to the UI Text element

    void Start()
    {
        // Initialize last position
        lastPosition = transform.position;
    }

    void Update()
    {
        // Calculate distance moved each frame
        float distanceMoved = Vector3.Distance(transform.position, lastPosition);
        totalDistance += distanceMoved;
        lastPosition = transform.position;
    }

    public void DisplayDistanceOnUI()
    {
        // Update the UI Text element with the total distance
        if (distanceText != null)
        {
            distanceText.text = "Distance walked: " + totalDistance.ToString("F2") + " meters";
        }
    }

    public float GetTotalDistance()
    {
        return totalDistance;
    }
}
