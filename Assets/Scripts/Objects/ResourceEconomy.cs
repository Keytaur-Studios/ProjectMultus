using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor.PackageManager;
using UnityEngine;
using static ResourceDictionary;

public class ResourceEconomy : NetworkBehaviour
{
    public static ResourceEconomy Instance { get; private set; }

    private Dictionary<int, int> resources = new();
    [SerializeField]
    private ResourceDictionary resDict;

    public static event Action<int, int> OnResourceCountChange;

    private void Awake()
    {
        Instance = this;

        foreach (ResourceEntry r in resDict.resources)
            resources.Add(r.id, 0);

        // Testing with banana
        resources[0] = 3;
        OnResourceCountChange?.Invoke(0, resources[0]);
        resources[1] = 3;
        OnResourceCountChange?.Invoke(1, resources[1]);
        resources[2] = 3;
        OnResourceCountChange?.Invoke(2, resources[2]);
        resources[3] = 3;
        OnResourceCountChange?.Invoke(3, resources[3]);
    }

    public int ResourceCount(int id)
    {
        return resources[id];
    }

    [ClientRpc]
    private void AddResourceClientRpc(int id, int amount)
    {
        resources[id] += amount;
        OnResourceCountChange?.Invoke(id, resources[id]);
    }

    [ClientRpc]
    private void RemoveResourceClientRpc(int id, int amount)
    {
        if (resources[id] >= amount)
            resources[id] -= amount;

        OnResourceCountChange?.Invoke(id, resources[id]);
    }

    [ServerRpc(RequireOwnership = false)]
    public void AddResourceServerRpc(int id, int amount)
    {
        AddResourceClientRpc(id, amount);
    }

    [ServerRpc(RequireOwnership = false)]
    public void RemoveResourceServerRpc(int id, int amount)
    {
        RemoveResourceClientRpc(id, amount);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnPrefabServerRpc(int id, ulong playerId, int shelfId)
    {
        GameObject spawned = Instantiate(resDict.GetResource(id));
        NetworkObject netObj = spawned.GetComponent<NetworkObject>();
        netObj.Spawn();

        SendSpawnedObjectClientRpc(netObj.NetworkObjectId, playerId, shelfId);
    }

    [ClientRpc]
    private void SendSpawnedObjectClientRpc(ulong objId, ulong playerId, int shelfId)
    {
        if (NetworkManager.Singleton.LocalClientId == playerId)
        {
            GameObject[] allObjects = FindObjectsByType<GameObject>(sortMode: FindObjectsSortMode.InstanceID);
            StorageShelf shelf = null;
            foreach (GameObject obj in allObjects)
            {
                if (obj.GetInstanceID() == shelfId)
                    shelf = obj.GetComponent<StorageShelf>();
            }

            shelf.OnResourceSpawned(objId);
        }
    }
}
