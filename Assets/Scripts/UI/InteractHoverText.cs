using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Rendering.FilterWindow;

// This class handles how to display labels when hovering on Interactable Objects
public class InteractHoverText : MonoBehaviour
{
    private PlayerMotor motor; // Publisher of hover events
    [SerializeField] GameObject hoverTextGameObject; // Hover text UI
    private UIDocument hoverTextUIDoc; // Visual tree component of hover text UI
    private Label hoverTextLabel; // The text that displays on hover
    


    void Awake()
    {
        // Initialize variables
        motor = gameObject.GetComponent<PlayerMotor>();
        hoverTextUIDoc = hoverTextGameObject.GetComponent<UIDocument>();
        hoverTextLabel = hoverTextUIDoc.rootVisualElement.Q<Label>("HoverText");

        hoverTextLabel.style.visibility = Visibility.Hidden; // ensures hovertext does not show when first spawning in

        // Subcribe to hover events
        if (motor != null)
        {
            motor.OnInteractableObjectAway += HideHoverText;
            motor.OnInteractableObjectHover += ShowHoverText;

        }
        else
        {
            Debug.Log("motor is null");
        }
    }
 
    // name holds the name of the interactable GameObject that the player is looking at
    public void ShowHoverText(string objectName) 
    {
        hoverTextLabel.text = objectName; 
        hoverTextLabel.style.visibility = Visibility.Visible;

    }

    public void HideHoverText()
    {
        hoverTextLabel.style.visibility = Visibility.Hidden;

    }

    void OnDisable()
    {
        motor.OnInteractableObjectHover -= ShowHoverText;
        motor.OnInteractableObjectAway -= HideHoverText;
    }
}


