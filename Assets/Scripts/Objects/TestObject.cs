using UnityEngine;

public class TestObject : InteractableObject
{
    public override void DisableText()
    {
        throw new System.NotImplementedException();
    }

    public override void EnableText()
    {
        throw new System.NotImplementedException();
    }

    override public void Interact()
    {
        Debug.Log("Interaction successful");
    }

    public override void StopInteract()
    {
        
    }
}
