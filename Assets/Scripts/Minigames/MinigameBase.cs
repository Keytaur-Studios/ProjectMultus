using System;
using UnityEngine;

public class MinigameBase : MonoBehaviour
{
    [SerializeField]
    private int minigameID;
    [SerializeField]
    private int percent = 0;

    public static event Action<int> OnMinigameComplete;

    public void SetPercent(int p)
    {
        percent = p;

        if (percent == 100)
            OnMinigameComplete?.Invoke(minigameID);
    }
}
