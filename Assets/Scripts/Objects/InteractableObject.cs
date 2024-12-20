using Unity.Netcode;
using UnityEngine;

abstract public class InteractableObject : NetworkBehaviour
{
    public bool isPressed;

    abstract public void Interact();
    
    abstract public void StopInteract();

}