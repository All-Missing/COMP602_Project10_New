using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonOutline : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Outline outline; // Reference to the Outline component

    void Start()
    {
        InitializeOutline();
    }

    // Initialize the outline (extracted from Start() for testability)
    public void InitializeOutline()
    {
        outline = GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = false; // Disable the outline by default
        }
    }

    // This function is called when the mouse enters the button
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (outline != null)
        {
            outline.enabled = true; // Enable the outline on hover
        }
    }

    // This function is called when the mouse exits the button
    public void OnPointerExit(PointerEventData eventData)
    {
        if (outline != null)
        {
            outline.enabled = false; // Disable the outline when not hovering
        }
    }
}
