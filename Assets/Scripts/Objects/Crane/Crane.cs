using Unity.Netcode;
using UnityEngine;

public class Crane : NetworkBehaviour
{
    [Header("Pieces")]
    public GameObject chain;
    public GameObject magnet;

    [ServerRpc (RequireOwnership = false)]
    public void DownServerRpc()
    {
        if (!IsHost)
            return;

        chain.transform.localScale += new Vector3(0, .01f, 0);
        Debug.Log("IN DOWN");
        magnet.transform.localPosition = new Vector3(magnet.transform.localPosition.x, magnet.transform.localPosition.y - .01f, magnet.transform.localPosition.z);
    }

    public void Down()
    {
        DownServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpServerRpc()
    {
        if (!IsHost)
            return;

        if (chain.transform.localScale.y <= 1)
            return;
        chain.transform.localScale += new Vector3(0, -.01f, 0);
        magnet.transform.localPosition = new Vector3(magnet.transform.localPosition.x, magnet.transform.localPosition.y + .01f, magnet.transform.localPosition.z);
    }

    [ServerRpc(RequireOwnership = false)]
    public void LeftServerRpc()
    {
        if (!IsHost)
            return;

        transform.Rotate(0, -1f, 0);
    }

    [ServerRpc(RequireOwnership = false)]
    public void RightServerRpc()
    {
        if (!IsHost)
            return;

        transform.Rotate(0, 1f, 0);
    }
}
