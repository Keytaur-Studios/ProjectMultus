using UnityEngine;

/* This class gives a field to write hover text for Interactable Objects. */

public class InteractableObjectInfo : MonoBehaviour
{
    // text to be displayed when player cursor is over object    
    private string hoverText;

    private void Start()
    {
        hoverText = gameObject.GetComponent<InteractableObject>().hoverText;
    }

    public string GetText()
    {
        return hoverText;
    }

}
