using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField] private int speed = 10;

    [ServerRpc]
    public void MoveServerRpc(Vector3 direction)
    {
        transform.position += direction;
        MoveClientRpc(direction);
    }

    [ClientRpc]
    public void MoveClientRpc(Vector3 direction)
    {
        if (IsHost || IsOwner) return;
        transform.position += direction;
    }

    private void Update()
    {
        if (!IsOwner) return;

        Vector3 direction = new Vector3();

        if (Input.GetKey(KeyCode.W)) direction.z = +1f;
        if (Input.GetKey(KeyCode.S)) direction.z = -1f;
        if (Input.GetKey(KeyCode.A)) direction.x = -1f;
        if (Input.GetKey(KeyCode.D)) direction.x = +1f;

        direction = speed * Time.deltaTime * direction;
        transform.position += direction;

        if (IsHost)
            MoveClientRpc(direction);
        else if (IsClient)
            MoveServerRpc(direction);

    }
}
