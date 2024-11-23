using UnityEngine;

public class CraneButton : InteractableObject
{
    public ButtonType buttonType;
    public Crane crane;

    public enum ButtonType
    {
        down, up, left, right
    }

    override public void Interact()
    {
        Debug.Log("Hit button");
        switch (buttonType)
        {
            case ButtonType.down:
                crane.Down();
                break;
            case ButtonType.up:
                crane.Up();
                break;
            case ButtonType.left:
                crane.Left();
                break;
            case ButtonType.right:
                crane.Right();
                break;
        }
    }
}
