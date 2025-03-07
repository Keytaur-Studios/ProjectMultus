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

    private string playerName;

    [SerializeField] GameObject mainMenuUI;
    [SerializeField] GameObject lobbyMenuUI;
    [SerializeField] GameObject editPlayerNamePopupUI;

    private Label playerNameText_main;
    private Label playerNameText_lobby;
    private Label playerNameText_popup;
    private Button editPlayerNameButton_main;
    private Button editPlayerNameButton_lobby;

    private TextField editPlayerNameInput;
    private Button okayButton;
    private Button cancelButton;


    private void Awake() {
        Instance = this;

        // generate playerName
        playerName = "Player" + UnityEngine.Random.Range(1000, 9999);
    }

    private void Start()
    {
        InitializeUI();
        UpdateNameOnUI();
        SubscribeToEvents();
    }

    private void InitializeUI()
    {
        // player name plate in main menu screen
        playerNameText_main = UIHelper.GetUIElement<Label>(mainMenuUI, "PlayerName");
        editPlayerNameButton_main = UIHelper.GetUIElement<Button>(mainMenuUI, "EditPlayerNameButton");
        // player name plate in lobby menu screen
        playerNameText_lobby = UIHelper.GetUIElement<Label>(lobbyMenuUI, "PlayerName");
        editPlayerNameButton_lobby = UIHelper.GetUIElement<Button>(lobbyMenuUI, "EditPlayerNameButton");
        // edit player name modal 
        editPlayerNameInput = UIHelper.GetUIElement<TextField>(editPlayerNamePopupUI, "EditPlayerNameInput");
        okayButton = UIHelper.GetUIElement<Button>(editPlayerNamePopupUI, "OkayButton");
        cancelButton = UIHelper.GetUIElement<Button>(editPlayerNamePopupUI, "CancelButton");
        playerNameText_popup = UIHelper.GetUIElement<Label>(editPlayerNamePopupUI, "PlayerNameTextOnModal");


        Debug.Log("edit init");
    }

    private void UpdateNameOnUI()
    {
        playerNameText_main.text = playerName;
        playerNameText_lobby.text = playerName;
        playerNameText_popup.text = playerName;
    }

    private void SubscribeToEvents()
    {
        editPlayerNameButton_lobby.clicked += ShowPopup;
        editPlayerNameButton_main.clicked += ShowPopup;
        okayButton.clicked += OnOkayButtonClick;
        cancelButton.clicked += OnCancelButtonClick;

        OnNameChanged += EditPlayerName_OnNameChanged;
    }


    private void ShowPopup()
    {

        UIHelper.ShowUI(editPlayerNamePopupUI, "EditPlayerNamePopup");
        editPlayerNameInput.Focus();
    }

    private void OnCancelButtonClick()
    {
        Debug.Log("edit click");

        // clear input field
        editPlayerNameInput.value = "";
        // hide popup
        UIHelper.HideUI(editPlayerNamePopupUI, "EditPlayerNamePopup");
    }

    private void OnOkayButtonClick()
    {
        Debug.Log("edit click");

        // Validate input
        if (editPlayerNameInput.value == "")
        {
            return; // do not apply; players cannot set an empty string as their name
        }

        // Valid input, reassign name
        playerName = editPlayerNameInput.value;
        UpdateNameOnUI();

        OnNameChanged?.Invoke(this, EventArgs.Empty);

        UIHelper.HideUI(editPlayerNamePopupUI, "EditPlayerNamePopup");

        // reset input field
        editPlayerNameInput.value = "";
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