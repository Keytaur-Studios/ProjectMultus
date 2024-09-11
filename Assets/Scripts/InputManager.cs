using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    public PlayerInput.PlayerControlsActions player;

    private PlayerMotor motor;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;

        playerInput = new PlayerInput();
        player = playerInput.PlayerControls;

        motor = GetComponent<PlayerMotor>();

        player.Click.performed += ctx => motor.Click();
        player.Jump.performed += ctx => motor.Jump();

        
        EnablePlayerControls();

    }

    void FixedUpdate() 
    {
        motor.ProcessMove(player.Move.ReadValue<Vector2>());
    }

    void LateUpdate()
    {
        motor.ProcessLook(player.Look.ReadValue<Vector2>());
    }


    // Enables the controls to move the player
    void EnablePlayerControls()
    {
        player.Enable();
    }

}