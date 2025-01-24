using TMPro;
using Unity.Netcode;
using UnityEngine;

abstract public class InteractableObject : NetworkBehaviour
{
    public bool isPressed;
    public string hoverText;

    abstract public void Interact(GameObject player);

    abstract public void StopInteract();

    public void EnableText()
    {
        GameObject.Find("InteractText").GetComponent<TextMeshProUGUI>().text = hoverText;
        // FINALIZE THIS GAMEOBJECT NAME
        GameObject.Find("InteractText").gameObject.SetActive(true);
    }

    public void DisableText()
    {
        GameObject.Find("InteractText").gameObject.SetActive(false);
    }

}