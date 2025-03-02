using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class EditPlayerName : MonoBehaviour
{
    public static EditPlayerName Instance { get; private set; }

    public event EventHandler OnNameChanged;

    //[SerializeField] private TextMeshProUGUI playerNameText;
    public GameObject mainMenuUI;
    public GameObject lobbyMenuUI;
    //public GameObject editPlayerNameUI;

    private Label playerNameText_main;
    private Label playerNameText_lobby;
    private Button editPlayerNameButton_main;
    private Button editPlayerNameButton_lobby;

    private string playerName;

    private void Awake() {
        Instance = this;


        // generate playerName
        playerName = "Player" + UnityEngine.Random.Range(1000, 9999);


        OnNameChanged += EditPlayerName_OnNameChanged;



    }

    private void Start()
    {
        playerNameText_main = mainMenuUI.GetComponent<UIDocument>().rootVisualElement.Q<Label>("PlayerName");
        editPlayerNameButton_main = mainMenuUI.GetComponent<UIDocument>().rootVisualElement.Q<Button>("EditPlayerNameButton");
        playerNameText_lobby = lobbyMenuUI.GetComponent<UIDocument>().rootVisualElement.Q<Label>("PlayerName");
        editPlayerNameButton_lobby = lobbyMenuUI.GetComponent<UIDocument>().rootVisualElement.Q<Button>("EditPlayerNameButton");

        // change name displayed on UI
        playerNameText_main.text = playerName;
        playerNameText_lobby.text = playerName;

        editPlayerNameButton_lobby.clicked += changeName;
        editPlayerNameButton_main.clicked += changeName;
    }

    private void changeName()
    {
        Debug.Log("change name button clicked detected");
        InputWindowUI.Show_Static("Player Name", playerName, "abcdefghijklmnopqrstuvxywzABCDEFGHIJKLMNOPQRSTUVXYWZ .,-", 20,
            () => {
                // Cancel
            },
            (string newName) => {
                playerName = newName;

                playerNameText_main.text = playerName;
                playerNameText_lobby.text = playerName;
                OnNameChanged?.Invoke(this, EventArgs.Empty);
            });
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