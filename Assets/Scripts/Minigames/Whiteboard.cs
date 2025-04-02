using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class Whiteboard : NetworkBehaviour
{
    public MinigameDict miniDict;

    [SerializeField]
    private List<TextMeshProUGUI> minigameStrings;

    [SerializeField]
    private List<Image> boxes;

    [SerializeField]
    private List<Image> checks;

    // Maps minigame ID to index in the list
    private Dictionary<int, int> idMapping= new();

    private int numItems = 0;

    private void Awake()
    {
        MinigameEconomy.OnMinigameAdded += WriteMinigame;
        MinigameBase.OnMinigameComplete += CompleteMinigameClientRpc;
    }

    private void WriteMinigame(int id)
    {
        Debug.Log("writing");
        boxes[numItems].gameObject.SetActive(true);
        minigameStrings[numItems].text = miniDict.GetResource(id);
        idMapping.Add(id, numItems);
        numItems++;
    }

    [ClientRpc]
    private void CompleteMinigameClientRpc(int id)
    {
        checks[idMapping[id]].gameObject.SetActive(true);
    }


}
