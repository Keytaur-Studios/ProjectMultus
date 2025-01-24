using UnityEngine;
using UnityEngine.UIElements;

// This class handles how to display labels when hovering on Interactable Objects
public class InteractHoverText : MonoBehaviour
{
    private PlayerMotor motor;
    [SerializeField] GameObject hoverText;


    void Awake()
    {
        motor = gameObject.GetComponent<PlayerMotor>();

        //hoverTextUI = hoverText.GetComponent<UIDocument>();

        hoverText.SetActive(false); // ensures hovertext does not show when first spawning in

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
    }
 
    public void ShowHoverText()
    {
        Debug.Log("showing");
        hoverText.SetActive(true);
        // display hovertext


    }

    public void HideHoverText()
    {
        //Debug.Log("hiding");
        hoverText.SetActive(false);
        // display hovertext
    }
}


