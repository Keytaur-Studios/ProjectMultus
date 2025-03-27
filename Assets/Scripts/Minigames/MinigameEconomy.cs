using System.Collections.Generic;
using UnityEngine;

public class MinigameEconomy : MonoBehaviour
{
    public static MinigameEconomy Instance { get; private set; }

    public Dictionary<int, int> minigames = new();

    private void Start()
    {
        Instance = this;

        // For now, fuel minigame will be considered minigame_0
        // Dictionary key is minigamenumber, and value is % done (for now either 100 or 0)

        minigames.Add(0, 0);

        MinigameBase.OnMinigameComplete += DebugPrintComplete;
    }

    public void DebugPrintComplete(int mID)
    {
        Debug.Log($"Minigame Complete {mID}");
    }
}
