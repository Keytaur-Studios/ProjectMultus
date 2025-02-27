using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    public PlayerInput.PlayerControlsActions player;
    public PlayerInput.MinigameControlsActions mini;

    private PlayerMotor motor;
    private PlayerLook look;

    private bool cameraFreeze;

    void Awake()
    {
        UnfreezeCamera();

        playerInput = new PlayerInput();
        player = playerInput.PlayerControls;
        mini = playerInput.MinigameControls;

        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();

        player.Click.performed += ctx => motor.Click();
        player.Jump.performed += ctx => motor.Jump();
        player.Interact.performed += ctx => look.InteractHandler();
        //player.Pause.performed += ctx => pauseMenu.TogglePauseMenu();

        EnablePlayerControls();

        MinigameBase.EnterMiniGame += MinigameEnter;
        MinigameBase.ExitMiniGame += MinigameExit;
    }

    private GameObject currentMinigame;

    void MinigameEnter(GameObject minigame)
    {
        FreezeCamera();
        DisableAllControls();
        EnableMinigameControls();
        currentMinigame = minigame;
        mini.Leave.performed += ctx => minigame.GetComponent<MinigameBase>().Leave();
    }

    void MinigameExit()
    {
        UnfreezeCamera();
        DisableAllControls();
        EnablePlayerControls();
        currentMinigame = null;
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
        mini.Disable();
    }

    // Enables the controls to move the player
    void EnablePlayerControls()
    {
        player.Enable();
    }

    void EnableMinigameControls()
    {
        mini.Enable();
    }
}