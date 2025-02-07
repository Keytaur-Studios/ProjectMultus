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

    [Header("Attached Player")]
    public GameObject attachedPlayer;
    public Transform playerCameraStorage;

    public static event Action<GameObject> EnterMiniGame;
    public static event Action ExitMiniGame;

    // Enter minigame state, move player camera.
    override public void Interact(GameObject player)
    {
        attachedPlayer = player;
        occupied = true;
        playerCameraStorage = player.GetComponent<PlayerLook>().cameraObj.transform;
        MoveCamera(player.GetComponent<PlayerLook>().cameraObj.transform, cameraTransform);
        InteractGame();
        EnterMiniGame?.Invoke(gameObject);
    }

    // Leave the minigame state. Reset minigame status to prepare for next use.
    public void Leave()
    {
        MoveCamera(attachedPlayer.GetComponent<PlayerLook>().cameraObj.transform, playerCameraStorage);

        // Prepare the minigame for the next player to use
        occupied = false; attachedPlayer = null; playerCameraStorage = null;

        ExitMiniGame?.Invoke();
    }

    // Specific interaction implementation for each minigame.
    abstract public void InteractGame();

    // Move camera to a new camera transform.
    public void MoveCamera(Transform camera, Transform newCameraTransform)
    {
        camera.SetPositionAndRotation(newCameraTransform.position, newCameraTransform.rotation);
    }
}
