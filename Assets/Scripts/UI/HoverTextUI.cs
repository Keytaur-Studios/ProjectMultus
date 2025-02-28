using UnityEngine;
using UnityEngine.UIElements;

/* This class handles displaying/hiding hover text when the player cursor is over an Interactable Object */
public class HoverText : MonoBehaviour
{
    private PlayerLook look; // Publisher of hover events
    [SerializeField] GameObject hoverTextGameObject; // Hover text UI
    private UIDocument hoverTextUIDocument; // Visual tree component of hover text UI
    private Label hoverTextContent; // The text that displays on hover


    void Awake()
    {
        // Initialize variables
        look = gameObject.GetComponent<PlayerLook>();
        hoverTextUIDocument = hoverTextGameObject.GetComponent<UIDocument>();
        hoverTextContent = hoverTextUIDocument.rootVisualElement.Q<Label>("HoverText");

        hoverTextContent.style.visibility = Visibility.Hidden; // ensures hovertext is hidden at the start

        // Subscribe to hover events
        look.OnInteractableHoverExit += HideHoverText;
        look.OnInteractableHoverEnter += ShowHoverText;

    }

    // hoverText is the hover text value defined in the InteractableObjectInfo component of an Interactable object
    public void ShowHoverText(string hoverText)
    {
        hoverTextContent.text = hoverText;
        hoverTextContent.style.visibility = Visibility.Visible;


    }

    public void HideHoverText()
    {
        hoverTextContent.style.visibility = Visibility.Hidden;

    }

    void OnDisable()
    {
        look.OnInteractableHoverExit -= HideHoverText;
        look.OnInteractableHoverEnter -= ShowHoverText;
    }
}


