using Unity.Netcode;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class FuelMixingDisplay : NetworkBehaviour
{
    [SerializeField]
    private Image displayedImage;
    [SerializeField]
    private List<Sprite> resourceSprites = new();

    public static event Action OnAllImagesDisplayed;

    private void Start()
    {
        FuelMixing.OnFuelMixStart += PrintComboClientRpc;

        displayedImage.sprite = null;
        displayedImage.enabled = false;
    }

    [ClientRpc]
    public void PrintComboClientRpc(int[] combo)
    {
        for (int i = 0; i < combo.Length; i++)
        {
            Debug.Log($"Item {i}: {combo[i]}");
        }

        StartCoroutine(DisplayComboImages(combo));
    }

    IEnumerator DisplayComboImages(int[] combo)
    {
        yield return new WaitForSeconds(1);

        for (int i = 0; i < combo.Length; i++)
        {
            displayedImage.enabled = true;
            displayedImage.sprite = resourceSprites[combo[i]];
            yield return new WaitForSeconds(1);
            displayedImage.enabled = false;
            yield return new WaitForSeconds(.2f);
        }

        displayedImage.sprite = null;
        displayedImage.enabled = false;

        OnAllImagesDisplayed?.Invoke();

        StopCoroutine(DisplayComboImages(combo));
    }
}
