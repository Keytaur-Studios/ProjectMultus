using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class FuelMixing : NetworkBehaviour
{
    [SerializeField]
    private MinigameBase MinigameBase;

    [SerializeField]
    private List<int> resourceRecipeIDs = new();
    private int[] comboAsArray;

    [SerializeField]
    private int correctResources = 0;

    [SerializeField]
    private int difficulty = 3;

    private int[] validChosenResources = { 0, 1, 2, 3 };

    public bool gameActive = false;
    public bool allowItems = false;

    public static event Action OnFuelMixFail;
    public static event Action OnFuelMixComplete;
    public static event Action OnFuelMixItemCorrect;
    public static event Action<int[]> OnFuelMixStart;

    private void Start()
    {
        FuelMixingDisplay.OnAllImagesDisplayed += AllowItemsServerRpc;
    }

    public void StartGame()
    {
        StartGameServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void StartGameServerRpc()
    {
        Debug.Log("GameStarted");
        for (int i = 0; i < difficulty; i++)
        {
            int n = UnityEngine.Random.Range(0, validChosenResources.Length);
            resourceRecipeIDs.Add(n);
        }

        comboAsArray = resourceRecipeIDs.ToArray();
        OnFuelMixStart?.Invoke(comboAsArray);
    }

    [ServerRpc(RequireOwnership = false)]
    public void AllowItemsServerRpc()
    {
        allowItems = true;
    }

    private void ItemCorrect()
    {
        correctResources++;
        OnFuelMixItemCorrect?.Invoke();
        if (correctResources == difficulty)
        {
            GetComponent<MinigameBase>().SetPercent(100);
            OnFuelMixComplete?.Invoke();
        }
    }

    private void Fail()
    {
        correctResources = 0;
        gameActive = false;
        resourceRecipeIDs.Clear();
        OnFuelMixFail?.Invoke();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (!NetworkManager.Singleton.IsHost) return;

        if (!collision.gameObject.CompareTag("Throwable Object")) return;

        if (!allowItems)
        {
            collision.gameObject.GetComponent<NetworkObject>().Despawn();
            return;
        }

        if (collision.gameObject.GetComponent<Resource>().GetResourceID()
                == resourceRecipeIDs[correctResources])
            ItemCorrect();
        else
            Fail();

        collision.gameObject.GetComponent<NetworkObject>().Despawn();
    }
}
