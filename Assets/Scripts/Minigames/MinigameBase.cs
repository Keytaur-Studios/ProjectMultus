using JetBrains.Annotations;
using System;
using Unity.VisualScripting;
using UnityEngine;

abstract public class MinigameBase : InteractableObject
{
    [Header("Info")]
    public string minigame;
    public bool occupied;

    [Header("Camera")]
    public Transform cameraTransform;

    [Header("Attached Plater")]
    public GameObject attachedPlayer;
    public Transform playerCameraStorage;

    // Enter minigame state, move player camera.
    override public void Interact(GameObject player)
    {
        attachedPlayer = player;
        occupied = true;
        //playerCameraStorage = player.GetComponent<PlayerMotor>().cameraObj.transform;
        //MoveCamera(player.GetComponent<PlayerMotor>().cameraObj.transform, cameraTransform);
        InteractGame();
    }

    // Leave the minigame state. Reset minigame status to prepare for next use.
    public void Leave()
    {
        //MoveCamera(attachedPlayer.GetComponent<PlayerMotor>().cameraObj.transform, playerCameraStorage);

        // Prepare the minigame for the next player to use
        occupied = false; attachedPlayer = null; playerCameraStorage = null;
    }

    // Specific interaction implementation for each minigame.
    abstract public void InteractGame();

    // Move camera to a new camera transform.
    public void MoveCamera(Transform camera, Transform newCameraTransform)
    {
        camera.SetPositionAndRotation(newCameraTransform.position, newCameraTransform.rotation);
    }
}
