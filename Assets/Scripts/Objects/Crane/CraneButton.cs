using UnityEngine;
using TMPro;

public class CraneButton : InteractableObject
{
    public ButtonType buttonType;
    public Crane crane;
    public TextMeshProUGUI buttonText;

    public enum ButtonType
    {
        down, up, left, right, in_, out_, magnetToggle
    }

    void Start()
    {
        isPressed = false;
        buttonText.text = buttonType.ToString();
        DisableText();
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
            case ButtonType.in_:
                crane.InServerRpc();
                break;
            case ButtonType.out_:
                crane.OutServerRpc();
                break;
            case ButtonType.magnetToggle:
                crane.MagnetToggleServerRpc();
                break;
        }
    }

    public override void EnableText() 
    {
        buttonText.enabled = true;
    }

    public override void DisableText()
    {
        buttonText.enabled = false;
    }
}
