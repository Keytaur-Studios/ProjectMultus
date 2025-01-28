using TMPro;
using Unity.Netcode;
using UnityEngine;

abstract public class InteractableObject : NetworkBehaviour
{
    public bool isPressed;
    public string hoverText;

    abstract public void Interact(GameObject player);

    abstract public void StopInteract();
}