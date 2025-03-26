using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class StorageReturner : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (!NetworkManager.Singleton.IsHost)
            return;

        if (!collision.gameObject.CompareTag("Throwable Object"))
            return;

        int resourceId = collision.gameObject.GetComponent<Resource>().GetResourceID();
        ResourceEconomy.Instance.AddResourceServerRpc(resourceId, 1);

        collision.gameObject.GetComponent<NetworkObject>().Despawn();
    }
}
