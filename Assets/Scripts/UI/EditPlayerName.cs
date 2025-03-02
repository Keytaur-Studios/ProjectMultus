using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditPlayerName : MonoBehaviour
{
    public static EditPlayerName Instance { get; private set; }

    public event EventHandler OnNameChanged;

    [SerializeField] private TextMeshProUGUI playerNameText;

    private string playerName;

    private void Awake() {
        Instance = this;

        // generate playerName
        playerName = "Player" + UnityEngine.Random.Range(1000, 9999);
        playerNameText.text = playerName;
        OnNameChanged += EditPlayerName_OnNameChanged;

        // create an inputwindow when clicked
        GetComponent<Button>().onClick.AddListener(() =>
        {
            InputWindowUI.Show_Static("Player Name", playerName, "abcdefghijklmnopqrstuvxywzABCDEFGHIJKLMNOPQRSTUVXYWZ .,-", 20,
            () => {
                // Cancel
            },
            (string newName) => {
                playerName = newName;

                playerNameText.text = playerName;

                OnNameChanged?.Invoke(this, EventArgs.Empty);
            });
        });

        playerNameText.text = playerName;
    }

    private void EditPlayerName_OnNameChanged(object sender, EventArgs e)
    {
        LobbyHandler.Instance.UpdatePlayerName(GetPlayerName());
    }

    public string GetPlayerName()
    {
        return playerName;
    }
}