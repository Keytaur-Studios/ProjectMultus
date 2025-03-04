using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ResourceDictionary;

public class MinigameEconomy : MonoBehaviour
{
    public static MinigameEconomy Instance { get; private set; }

    [SerializeField]
    private List<MinigameDictionary> miniDicts = new();
    [SerializeField]
    private List<ResourceEntry> assignedGames = new();

    public void AssignGam  es(int[] numPerCategory)
}
