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

    private Dictionary<int, int> resources = new Dictionary<int, int>();
    [SerializeField]
    private ResourceDictionary resDict;

    public static event Action<int, int> OnResourceCountChange;

    private void Start()
    {
        Instance = this;

        foreach (ResourceEntry r in resDict.resources)
            resources.Add(r.id, 0);

        resources[0] = 3;
        OnResourceCountChange?.Invoke(0, resources[0]);
    }

    public int ResourceCount(int id)
    {
        if (resources.ContainsKey(id)) 
            return resources[id];
        else
        {
            Debug.Log($"id: {id} not present in dictionary");
            return 0;
        }
    }

    [ClientRpc]
    private void AddResourceClientRpc(int id, int amount)
    {
        if (resources.ContainsKey(id))
            resources[id] += amount;
        else
            Debug.Log($"id: {id} not present in dictionary");

        OnResourceCountChange?.Invoke(id, resources[id]);
    }

    [ClientRpc]
    private void RemoveResourceClientRpc(int id, int amount)
    {
        Debug.Log("In rpc");
        if (resources.ContainsKey(id))
        {
            if (resources[id] >= amount)
                resources[id] -= amount;
            else
                Debug.Log($"Trying to remove {amount} when there are only {resources[id]} available");
        }
        else
            Debug.Log($"id: {id} not present in dictionary");

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
    public void SpawnPrefabServerRpc(int id, ulong playerId)
    {
        GameObject spawned = Instantiate(resDict.GetResource(id));
        NetworkObject netObj = spawned.GetComponent<NetworkObject>();
        netObj.Spawn();

        SendSpawnedObjectClientRpc(netObj.NetworkObjectId, playerId);
    }

    [ClientRpc]
    private void SendSpawnedObjectClientRpc(ulong objId, ulong playerId)
    {
        if (NetworkManager.Singleton.LocalClientId == playerId)
        {
            StorageShelf shelf = FindFirstObjectByType<StorageShelf>();
            if (shelf != null)
                shelf.OnResourceSpawned(objId);
        }
    }
}
