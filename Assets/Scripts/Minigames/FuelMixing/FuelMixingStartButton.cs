using UnityEngine;

public class FuelMixingStartButton : InteractableObject
{
    [SerializeField]
    private FuelMixing FuelMixing;

    public override void Interact(GameObject player)
    {
        if (!FuelMixing.gameActive)
            FuelMixing.StartGame();
    }

    public override void StopInteract()
    {
        throw new System.NotImplementedException();
    }
}
