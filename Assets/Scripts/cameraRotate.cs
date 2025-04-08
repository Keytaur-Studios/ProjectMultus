using UnityEngine;

public class cameraRotate : MonoBehaviour
{
    Transform orientation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        orientation = transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        orientation.Rotate(0, 0.05f, 0, Space.World);
    }
}
