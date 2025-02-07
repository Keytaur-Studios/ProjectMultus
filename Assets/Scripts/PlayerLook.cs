using UnityEngine;
using Unity.Netcode;
using System;

public class PlayerLook : NetworkBehaviour
{
    private PlayerMotor motor;

    // events to trigger when cursor is or isn't on an interactable object
    public delegate void OnInteractableHoverEnterDelegate(string objectName); // 
    public event OnInteractableHoverEnterDelegate OnInteractableHoverEnter; // triggers when player cursor is over an interactable object
    public event Action OnInteractableHoverExit; // triggers when player cursor is NOT over an interactable object

    [Header("Camera")]
    public GameObject cameraObj;

    [Header("Look")]
    public Camera cam;
    private Vector2 rotation = Vector2.zero;
    public float xSensitivity = 30f;
    public float ySensitivity = 30f;
    [Range(0, 90f)][SerializeField] float yRotationLimit = 60f;

    [Header("Interaction")]
    public InteractableObject target;
    public InteractableObject lastInteracted;
    public LayerMask targetMask;
    public string interactableHoverText;

    
    
    void Start() {
        motor = GetComponent<PlayerMotor>();

        // IsOwner does not get set to True in Awake for some reason
        if (IsOwner && motor.online == PlayerMotor.OnlineState.online)
            cam.gameObject.SetActive(true);
    }

    public override void OnNetworkSpawn() {
        base.OnNetworkSpawn();
        if (IsOwner) 
        {
            // Adjust the camera culling mask to ignore the LocalPlayer layer
            if (cam != null)
            {
                cam.cullingMask &= ~(1 << LayerMask.NameToLayer("Local Player"));
            }
        }
    }

    void FixedUpdate()
    {
        if (target != lastInteracted && lastInteracted != null)
            lastInteracted.StopInteract();
    }

    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;

        if (!IsOwner && motor.online == PlayerMotor.OnlineState.online)
            return;

        rotation.x += mouseX * xSensitivity * Time.deltaTime;
        rotation.y += mouseY * ySensitivity * Time.deltaTime;
        rotation.y = Mathf.Clamp(rotation.y, -yRotationLimit, yRotationLimit);
        cam.transform.localRotation = Quaternion.Euler(-rotation.y, 0, 0);
        transform.localRotation = Quaternion.Euler(0, rotation.x, 0);

        // Check if looking at Interactable Object
        CheckForTarget();
    }

    public void CheckForTarget()
    {
        bool isHit = Physics.Raycast(cam.transform.position, cam.transform.rotation * Vector3.forward, out RaycastHit hitInfo, 5f, targetMask);

        if (isHit)
            Debug.DrawLine(cam.transform.position, hitInfo.point, Color.red);
        else
            Debug.DrawLine(cam.transform.position, cam.transform.position + (cam.transform.rotation * Vector3.forward * 5f), Color.red);

        if (!isHit)
        {
            OnInteractableHoverExit?.Invoke();
            target = null;
            return;
        }

        if (!hitInfo.transform.gameObject.CompareTag("Interactable Object"))
        {
            // Player is not hovering over an Interactable Object
            OnInteractableHoverExit?.Invoke();
            return;
        }

        interactableHoverText = hitInfo.transform.gameObject.GetComponent<InteractableObject>().GetText();
        //interactableHoverText = hitInfo.transform.gameObject.GetComponent<InteractableObjectInfo>().GetText();

        // If player is hovering over an Interactable Object, then trigger event
        OnInteractableHoverEnter?.Invoke(interactableHoverText);

        target = hitInfo.transform.gameObject.GetComponent<InteractableObject>();
        //target.EnableText();

        //if (target != lastInteracted && lastInteracted != null)
            //lastInteracted.StopInteract();
    }

    public void Interact()
    {
        if (target == null || (!IsOwner && motor.online == PlayerMotor.OnlineState.online))
            return;

        target.Interact(gameObject);
        lastInteracted = target;
    }

    //public void StopInteract()
    //{
    //    if (target == null || (!IsOwner && motor.online == PlayerMotor.OnlineState.online))
    //        return;

    //    lastInteracted.StopInteract();
    //}
}
