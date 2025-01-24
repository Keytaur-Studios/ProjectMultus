using UnityEngine;
using UnityEngine.UIElements;

// This class handles how to display labels when hovering on Interactable Objects
public class InteractHoverText : MonoBehaviour
{
    private PlayerMotor motor; // Publisher of hover events
    [SerializeField] GameObject hoverTextGameObject; // Hover text UI
    private UIDocument hoverTextUIDoc; // Visual tree of hover text UI
    private Label hoverTextLabel; // The text that displays on hover


    void Awake()
    {
        // Initialize variables
        motor = gameObject.GetComponent<PlayerMotor>();
        hoverTextUIDoc = hoverTextGameObject.GetComponent<UIDocument>();
        hoverTextLabel = hoverTextUIDoc.rootVisualElement.Q("Overlay").Q<Label>("HoverText");

        hoverTextGameObject.SetActive(false); // ensures hovertext does not show when first spawning in

        //Debug.Log("interacthovertext.cs is awake");
        if (motor != null)
        {
            motor.OnInteractableObjectAway += HideHoverText;
            motor.OnInteractableObjectHover += ShowHoverText;
            Debug.Log("events subscribed!");

        }
        else
        {
            Debug.Log("motor is null");
        }
    }

    void OnDisable()
    {
        motor.OnInteractableObjectHover -= ShowHoverText;
        motor.OnInteractableObjectAway -= HideHoverText;
    }
 
    // name holds the name of a interactable GameObject that the player is looking at
    public void ShowHoverText(string objectName) 
    {
         // changes text depending on what player is looking at
        hoverTextGameObject.SetActive(true);
        hoverTextLabel.text = objectName;
        Debug.Log(hoverTextLabel.text);

        // display hovertext


    }

    public void HideHoverText()
    {
        hoverTextGameObject.SetActive(false);
    }
}


