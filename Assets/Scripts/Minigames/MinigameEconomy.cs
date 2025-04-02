using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MinigameEconomy : NetworkBehaviour
{
    public static MinigameEconomy Instance { get; private set; }

    public Dictionary<int, int> minigames = new();

    public static event Action<int> OnMinigameAdded;

    private void Start()
    {
        Instance = this;

        // For now, fuel minigame will be considered minigame_0
        // Dictionary key is minigamenumber, and value is % done (for now either 100 or 0)

        MinigameAdd(0);

        MinigameBase.OnMinigameComplete += DebugPrintComplete;
    }

    public void DebugPrintComplete(int mID)
    {
        DebugPrintCompleteClientRpc(mID);
    }

    private void MinigameAdd(int id)
    {
        minigames.Add(id, 0);
        OnMinigameAdded?.Invoke(id);
        Debug.Log("Add minigame");
    }

    [ClientRpc]
    public void DebugPrintCompleteClientRpc(int mID)
    {
        Debug.Log($"Minigame Complete {mID}");
    }
}
