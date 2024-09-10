using Unity.Netcode;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    public PlayerInput.PlayerControlsActions player;

    public GameObject playerObject;

    private PlayerMotor motor;
    private NetworkManager network;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;

        playerInput = new PlayerInput();
        player = playerInput.PlayerControls;

        motor = GetComponent<PlayerMotor>();
        network = GetComponent<NetworkManager>();

        player.Click.performed += ctx => motor.Click();
        player.Jump.performed += ctx => motor.Jump();

        player.Enable();

    }

    void FixedUpdate() 
    {
        motor.ProcessMove(player.Move.ReadValue<Vector2>());
    }

    void LateUpdate()
    {
        motor.ProcessLook(player.Look.ReadValue<Vector2>());
    }


}