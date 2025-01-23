using UnityEngine;

public class TestGame : MinigameBase
{
    private void Start()
    {
        minigame = "Test";
        hoverText = "Test the Game";
        cameraTransform.position = new Vector3();
        cameraTransform.rotation = new Quaternion(0, 0, 0, 0);
    }


}
