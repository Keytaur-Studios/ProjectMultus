using UnityEngine;
using Unity.Netcode;

public class PlayerMotor : NetworkBehaviour
{
    private Rigidbody playerRigidbody;
    private Vector3 moveDirection;
    public MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        crouch,
        air
    }

    [Header("Look")]
    public Camera cam;
    private Vector2 rotation = Vector2.zero;
    public float xSensitivity = 30f;
    public float ySensitivity = 30f;
    [Range(0, 90f)][SerializeField] float yRotationLimit = 88f;

    [Header("Slope Handler")]
    public float maxSlopeAngle; // Max angle of a slope the player can walk on
    private RaycastHit slopeHit; // Raycast for detecting when on a slope

    [Header("Grounded Check")]
    //public float playerHeight = 0; // How tall the player is. Important for the ground check raycast // removing orientation defeats the need of this var
    public LayerMask whatIsGround; // The layermask for the raycast to use for detecting the ground
    public bool isGrounded; // True when player is on a surface recognized as ground

    [Header("Ground Control")]
    public float walkspeed; // The speed the player goes when grounded
    public float sprintSpeed; // The speed the player goes when sprinting
    public float groundDrag = 10f; // Drag applied to the player when on the ground // was 0.5f
    public float sprintSpeedPercent = 0.5f; // What percent to increase speed by when sprinting
    public float acceleration = 10f; // How much to accelerate the player by. Does not affect max speed
    private float speed; // Also acts as the max speed for the player

    [Header("Air Control")]
    //public float airSpeed = 15f; // Acts as the max speed when the player is in the air
    public float airDrag = 1; // Drag applied to the player when in the air
    public float airMultiplier = 0.25f; // Affects how strong the player's control is in the air (lower means less control)
    public float jumpHeight = 0.75f; // How powerful or high the player's jumps are
    public float gravity = -15f; // Gravity
    public float airAccel = 10f; // How much to accelerate the player by when in the air. Does not affect max speed

    void Awake() {
        playerRigidbody = GetComponent<Rigidbody>();
        playerRigidbody.maxAngularVelocity = 0;

        
        
    }

    void Start()
    {
        // IsOwner does not get set to True in Awake for some reason
        if (IsOwner) 
            cam.gameObject.SetActive(true);
    }

    void FixedUpdate()
    {
        // Grounded Check
        //isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.1f, whatIsGround);
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.1f, whatIsGround);

        // Modulate gravity
        if(playerRigidbody.useGravity)
            playerRigidbody.AddForce(Vector3.down * (-1*gravity-9.8f), ForceMode.Force);

        // Apply the appropriate drag value
        if (isGrounded)
            playerRigidbody.linearDamping = groundDrag;
        else
            playerRigidbody.linearDamping = airDrag;

        // Limit velocity to speed variable
        SpeedControl();

        // Change the state and speed to the appropriate value
        StateHandler();
    }
    
    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;

        rotation.x += mouseX * xSensitivity * Time.deltaTime;
        rotation.y += mouseY * ySensitivity * Time.deltaTime;
        rotation.y = Mathf.Clamp(rotation.y, -yRotationLimit, yRotationLimit);
        //cam.transform.localRotation = Quaternion.Euler(-rotation.y, 0, 0);
        transform.localRotation = Quaternion.Euler(0, rotation.x, 0);
    }

    public void ProcessMove(Vector2 input)
    {
        // Vector3 moveDirection = transform.forward * input.y + transform.right * input.x;
        // transform.position += movementSpeed * Time.deltaTime * moveDirection;

        // Find the movement direction from input
        moveDirection = transform.forward * input.y + transform.right * input.x;

        // On a slope
        if (OnSlope())
        {
            playerRigidbody.AddForce(GetSlopeMoveDirection() * speed * acceleration * 2f, ForceMode.Force);

            if(playerRigidbody.linearVelocity.y > 0)
            {
                playerRigidbody.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }

        // Grounded
        else if(isGrounded)
            playerRigidbody.AddForce(moveDirection.normalized * speed * acceleration, ForceMode.Force);

        // In air use airMultiplier
        else if(!isGrounded)
            playerRigidbody.AddForce(moveDirection.normalized * speed * airMultiplier * airAccel, ForceMode.Force);

        
        // Turn off gravity when on a slope
        playerRigidbody.useGravity = !OnSlope() && !isGrounded;


        //network.MoveServerRpc(transform.position);
    }

    public void Click() {

    }

    public void Jump() // Applies a force upwards to jump. Affected by jumpHeight and jumpCooldown
    {
        if(isGrounded) 
        {
            playerRigidbody.linearVelocity = new Vector3(playerRigidbody.linearVelocity.x, 0f, playerRigidbody.linearVelocity.z);
            playerRigidbody.AddForce(10f * jumpHeight * transform.up, ForceMode.Impulse);
        }
    }

    private void StateHandler()
    {
        // // Crouching
        // if (crouching)
        // {
        //     state = MovementState.crouch;
        //     speed = crouchSpeed;
        // }

        // // Sprinting
        // else if(isGrounded && sprinting)
        // {
        //     state = MovementState.sprinting;
        //     speed = sprintSpeed;
        // }

        // // Walking
        // else 
        if (isGrounded)
        {
            state = MovementState.walking;
            speed = walkspeed;
        }

        // Air
        else
        {
            state = MovementState.air;
        }
    }

    private void SpeedControl() // Limits the player's speed to the speed variable
    {
        // Limiting the velocity while on a slope
        if (OnSlope())
        {
            if (playerRigidbody.linearVelocity.magnitude > speed)
            {
                playerRigidbody.linearVelocity = playerRigidbody.linearVelocity.normalized * speed;
            }
        }

        else
        {
            Vector3 flatVel = new Vector3(playerRigidbody.linearVelocity.x, 0f, playerRigidbody.linearVelocity.z);

            // Limit velocity
            if (flatVel.magnitude > speed) /*|| (flatVel.magnitude > airSpeed) && !isGrounded*/
            {
                Vector3 limitedVel = flatVel.normalized * speed; /*(isGrounded ? speed : airSpeed)*/
                playerRigidbody.linearVelocity = new Vector3(limitedVel.x, playerRigidbody.linearVelocity.y, limitedVel.z);
            }
        }
    }

    private bool OnSlope()
    {
        //if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

}