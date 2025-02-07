using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    public PlayerInput.PlayerControlsActions player;

    private PlayerMotor motor;
    private PlayerLook look;

    private bool cameraFreeze;

    void Awake()
    {
        UnfreezeCamera();

        playerInput = new PlayerInput();
        player = playerInput.PlayerControls;

        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();

        player.Click.performed += ctx => motor.Click();
        player.Jump.performed += ctx => motor.Jump();
        player.Interact.performed += ctx => look.Interact();
        player.Interact.canceled += ctx => look.StopInteract();

        EnablePlayerControls();

        MinigameBase.EnterMiniGame += MinigameEnter;
        MinigameBase.ExitMiniGame += MinigameExit;
    }

    void MinigameEnter(GameObject minigame)
    {
        FreezeCamera();
        DisableAllControls();
        EnableMinigameControls();
    }

    void MinigameExit()
    {
        UnfreezeCamera();
        DisableAllControls();
        EnablePlayerControls();
    }

    void FreezeCamera()
    {
        cameraFreeze = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    void UnfreezeCamera()
    {
        cameraFreeze = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate() 
    {
        motor.ProcessMove(player.Move.ReadValue<Vector2>());
    }

    void LateUpdate()
    {
        if (!cameraFreeze)
            look.ProcessLook(player.Look.ReadValue<Vector2>());
    }

    void DisableAllControls()
    {
        player.Disable();
        // mingame here
    }

    // Enables the controls to move the player
    void EnablePlayerControls()
    {
        player.Enable();
    }

    void EnableMinigameControls()
    {
        // minigame here
    }
}