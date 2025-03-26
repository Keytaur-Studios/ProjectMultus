using TMPro;
using Unity.Netcode;
using UnityEngine;

abstract public class InteractableObject : MonoBehaviour
{
    public bool isPressed;
    [SerializeField]
    private string hoverText;

    abstract public void Interact(GameObject player);

    abstract public void StopInteract();

    public string GetText()
    {
        return hoverText;
    }
}