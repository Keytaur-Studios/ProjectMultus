using Unity.Netcode;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] Camera targetCamera;
    Transform player;

    private void Start()
    {
        player = NetworkManager.Singleton.LocalClient.PlayerObject.transform;
        targetCamera = player.Find("Camera").GetComponent<Camera>();
        if (targetCamera == null)
        {
            Debug.LogError("player camera is null!");
        }
    }

    private void Update()
    {
        // always rotate to face the camera
        if (targetCamera != null)
        {
            transform.LookAt(targetCamera.transform);
            transform.Rotate(0, 180, 0);
        }
    }
}
