using UnityEngine;

public class CraneButton : InteractableObject
{
    public ButtonType buttonType;
    public Crane crane;

    public enum ButtonType
    {
        down, up, left, right
    }

    void Start()
    {
        isPressed = false;
    }

    private void FixedUpdate()
    {
        if (isPressed)
            ButtonCommands();
    }

    override public void Interact()
    {
        isPressed = true;
        Debug.Log("Hit button");
    }

    public override void StopInteract()
    {
        isPressed = false;
    }

    public void ButtonCommands()
    {
        switch (buttonType)
        {
            case ButtonType.down:
                crane.Down();
                break;
            case ButtonType.up:
                crane.UpServerRpc();
                break;
            case ButtonType.left:
                crane.LeftServerRpc();
                break;
            case ButtonType.right:
                crane.RightServerRpc();
                break;
        }
    }
}
