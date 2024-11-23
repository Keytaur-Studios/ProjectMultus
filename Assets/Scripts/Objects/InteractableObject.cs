using Unity.Netcode;
using UnityEngine;

abstract public class InteractableObject : NetworkBehaviour
{
    abstract public void Interact();

}