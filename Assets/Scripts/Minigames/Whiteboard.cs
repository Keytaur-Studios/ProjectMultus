using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Whiteboard : MonoBehaviour
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

    private void Start()
    {
        MinigameEconomy.OnMinigameAdded += WriteMinigame;
        MinigameBase.OnMinigameComplete += CompleteMinigame;
    }

    private void WriteMinigame(int id)
    {
        boxes[numItems].gameObject.SetActive(true);
        minigameStrings[numItems].text = miniDict.GetResource(id);
        idMapping.Add(id, numItems);
        numItems++;
    }

    private void CompleteMinigame(int id)
    {
        checks[idMapping[id]].enabled = true;
    }


}
