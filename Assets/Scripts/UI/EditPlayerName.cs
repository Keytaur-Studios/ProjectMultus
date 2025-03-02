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
    public GameObject editPlayerNamePopupUI;

    private Label playerNameText_main;
    private Label playerNameText_lobby;
    private Label playerNameText_popup;
    private Button editPlayerNameButton_main;
    private Button editPlayerNameButton_lobby;

    private TextField editPlayerNameInput;
    private Button okayButton;
    private Button cancelButton;

    private string playerName;

    private void Awake() {
        Instance = this;


        // generate playerName
        playerName = "Player" + UnityEngine.Random.Range(1000, 9999);


        OnNameChanged += EditPlayerName_OnNameChanged;



    }

    private void Start()
    {
        // main menu's edit name ui
        playerNameText_main = mainMenuUI.GetComponent<UIDocument>().rootVisualElement.Q<Label>("PlayerName");
        editPlayerNameButton_main = mainMenuUI.GetComponent<UIDocument>().rootVisualElement.Q<Button>("EditPlayerNameButton");
        
        // lobby menu's edit name ui
        playerNameText_lobby = lobbyMenuUI.GetComponent<UIDocument>().rootVisualElement.Q<Label>("PlayerName");
        editPlayerNameButton_lobby = lobbyMenuUI.GetComponent<UIDocument>().rootVisualElement.Q<Button>("EditPlayerNameButton");
        
        // editPlayerNamePopupUI UI Document references
        editPlayerNameInput = editPlayerNamePopupUI.GetComponent<UIDocument>().rootVisualElement.Q<TextField>("EditPlayerNameInput");
        okayButton = editPlayerNamePopupUI.GetComponent<UIDocument>().rootVisualElement.Q<Button>("OkayButton");
        cancelButton = editPlayerNamePopupUI.GetComponent<UIDocument>().rootVisualElement.Q<Button>("CancelButton");
        playerNameText_popup = editPlayerNamePopupUI.GetComponent<UIDocument>().rootVisualElement.Q<Label>("PlayerNameTextOnModal");

        // change name displayed on UI
        playerNameText_main.text = playerName;
        playerNameText_lobby.text = playerName;
        playerNameText_popup.text = playerName;

        editPlayerNameButton_lobby.clicked += ShowPopup;
        editPlayerNameButton_main.clicked += ShowPopup;
        okayButton.clicked += OnOkayButtonClick;
        cancelButton.clicked += OnCancelButtonClick;

    }

    private void ShowPopup()
    {
        MainMenuUI.ShowUI(editPlayerNamePopupUI, "EditPlayerNamePopup");
        


        /*
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

        */
    }

    private void OnCancelButtonClick()
    {
        // clear input field
        editPlayerNameInput.value = "";
        // hide popup
        MainMenuUI.HideUI(editPlayerNamePopupUI, "EditPlayerNamePopup");
    }

    private void OnOkayButtonClick()
    {
        // Validate input; will not apply if empty
        if (editPlayerNameInput.value == "")
        {
            return;
        }

        // Valid input, reassign name
        playerName = editPlayerNameInput.value;
        playerNameText_main.text = playerName;
        playerNameText_lobby.text = playerName;
        playerNameText_popup.text = playerName;

        OnNameChanged?.Invoke(this, EventArgs.Empty);

        // clear input field
        editPlayerNameInput.value = "";
       
        MainMenuUI.HideUI(editPlayerNamePopupUI, "EditPlayerNamePopup");
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