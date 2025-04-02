using Unity.Netcode;
using UnityEngine;
using TMPro;

public class StorageShelf : InteractableObject
{
    [SerializeField]
    private int resourceId;
    [SerializeField]
    private GameObject resourcePrefab;
    [SerializeField]
    private TextMeshProUGUI countText;

    public int c;
    private GameObject playerObj;

    private void Awake()
    {
        ResourceEconomy.OnResourceCountChange += UpdateSignCount;
    }

    public override void Interact(GameObject player)
    {
        if (ResourceEconomy.Instance.ResourceCount(resourceId) <= 0)
            return;

        playerObj = player;

        ResourceEconomy.Instance.RemoveResourceServerRpc(resourceId, 1);
        ResourceEconomy.Instance.SpawnPrefabServerRpc(resourceId, NetworkManager.Singleton.LocalClientId, gameObject.GetInstanceID());
    }

    public void OnResourceSpawned(ulong objId)
    {
        NetworkObject netObj = NetworkManager.Singleton.SpawnManager.SpawnedObjects[objId];
        if (netObj != null)
        {
            GameObject spawned = netObj.gameObject;
            spawned.GetComponent<ThrowableObject>().PickUp(playerObj);
            playerObj = null;
        }
    }

    public override void StopInteract()
    {
        throw new System.NotImplementedException();
    }

    private void UpdateSignCount(int id, int count)
    {
        if (!(id == resourceId)) return;

        c = count;
        countText.text = count.ToString();
    }

}
