using JetBrains.Annotations;
using System;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

abstract public class MinigameBase : InteractableObject
{
    [Header("Info")]
    public string minigame; 
    public bool occupied;

    [Header("Camera")]
    public Transform cameraTransform;

    [Header("Attached Player")]
    public GameObject attachedPlayer;
    public GameObject playerCamera;
    public float cameraOffset;

    private bool interactCooldown = false;

    public static event Action<GameObject> EnterMiniGame;
    public static event Action ExitMiniGame;

    // Enter minigame state, move player camera.
    override public void Interact(GameObject player)
    {
        // Don't enter if just left
        if (interactCooldown)
            return;

        interactCooldown = true;
        occupied = true;

        // Get the player's camera
        playerCamera = player.GetComponent<PlayerLook>().cameraObj;
        // Get the height of the camera relative to the player
        cameraOffset = playerCamera.transform.localPosition.y;
        // Store the player
        attachedPlayer = player;
        // Set the camera to be parentless
        playerCamera.transform.SetParent(null);

        // Move the camera to the minigame view
        MoveCamera(playerCamera.transform, cameraTransform);

        InteractGame();
        EnterMiniGame?.Invoke(gameObject);
    }

    // Leave the minigame state. Reset minigame status to prepare for next use.
    public void Leave()
    {
        // Set the camera's parent back to the player, then move back to player
        playerCamera.transform.SetParent(attachedPlayer.transform);
        ReturnCamera(playerCamera.transform);

        // Prepare the minigame for the next player to use
        occupied = false; attachedPlayer = null; playerCamera = null;

        ExitMiniGame?.Invoke();
        StartCoroutine(StartInteractCooldown());
    }

    // Specific interaction implementation for each minigame.
    abstract public void InteractGame();

    // Move camera to a new camera transform.
    public void MoveCamera(Transform camera, Transform newCameraTransform) 
    {
        camera.SetPositionAndRotation(newCameraTransform.position, newCameraTransform.rotation);
    }

    // Move camera to position (0, cameraOffset, 0) relative to the player transform
    public void ReturnCamera(Transform camera) 
    {
        camera.SetLocalPositionAndRotation(Vector3.up * cameraOffset, Quaternion.identity);
    }

    // This is to prevent auto returning to minigame
    private IEnumerator StartInteractCooldown()
    {
        yield return new WaitForSeconds(.1f);
        interactCooldown = false;
    }
}
