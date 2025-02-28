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
    public GameObject target;
    public GameObject lastInteracted;
    public LayerMask targetMask;
    public string interactableHoverText;

    public GameObject hand;
    
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

    //void FixedUpdate()
    //{
    //    if (target != lastInteracted && lastInteracted != null)
    //        lastInteracted.GetComponent<StopInteract();
    //}

    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;

        if (!IsOwner && motor.online == PlayerMotor.OnlineState.online)
            return;

        // Stop camera movement while in pause menu
        if (PauseMenuUI.isGamePaused)
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

        if (!(hitInfo.transform.gameObject.CompareTag("Interactable Object")
            || hitInfo.transform.gameObject.CompareTag("Throwable Object")))
        {
            // Player is not hovering over an Interactable Object
            OnInteractableHoverExit?.Invoke();
            return;
        }

        if (hitInfo.transform.gameObject.CompareTag("Interactable Object"))
            interactableHoverText = hitInfo.transform.gameObject.GetComponent<InteractableObject>().GetText();

        if (hitInfo.transform.gameObject.CompareTag("Throwable Object"))
            interactableHoverText = hitInfo.transform.gameObject.GetComponent<ThrowableObject>().GetText();
        //interactableHoverText = hitInfo.transform.gameObject.GetComponent<InteractableObjectInfo>().GetText();

        // If player is hovering over an Interactable Object, then trigger event
        OnInteractableHoverEnter?.Invoke(interactableHoverText);

        target = hitInfo.transform.gameObject;
        //target.EnableText();

        //if (target != lastInteracted && lastInteracted != null)
            //lastInteracted.StopInteract();
    }

    public void SecondaryInteractHandler()
    {
        if (!IsOwner) return;

        if (lastInteracted == null) return;

        if (lastInteracted.CompareTag("Throwable Object"))
            lastInteracted.GetComponent<ThrowableObject>().Throw();


        lastInteracted = null;
    }
   
    public void InteractHandler()
    {
        if (!IsOwner) return;

        if (lastInteracted != null)
            StopInteract();

        if (target == null) return;

        if (target.CompareTag("Interactable Object"))
            Interact();

        else if (target.CompareTag("Throwable Object"))
        {
            if (!target.GetComponent<ThrowableObject>().GetOwned())
                Interact();
            else
                StopInteract();
        }
    }

    public void Interact()
    {
        if (target == null || (!IsOwner && motor.online == PlayerMotor.OnlineState.online))
            return;

        if (target.CompareTag("Interactable Object"))
            target.GetComponent<InteractableObject>().Interact(gameObject);

        if (target.CompareTag("Throwable Object"))
            target.GetComponent<ThrowableObject>().PickUp(gameObject);

        lastInteracted = target;
    }

    public void StopInteract()
    {
        if (lastInteracted == null || (!IsOwner && motor.online == PlayerMotor.OnlineState.online))
            return;

        if (lastInteracted.CompareTag("Throwable Object"))
            lastInteracted.GetComponent<ThrowableObject>().Drop();

        lastInteracted = null;
    }
}
