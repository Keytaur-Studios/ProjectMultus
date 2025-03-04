using System;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuUI : MonoBehaviour
{
    public static MainMenuUI Instance { get; private set; }


    public GameObject mainMenuUI;
    public GameObject lobbyMenuUI;
    public GameObject joinCodePopupUI;
    public GameObject editPlayerNamePopupUI;
    public GameObject settingsMenuUI;

    private Button joinGameButton, createLobbyButton, optionsButton, exitButton, editPlayerNameButton;

    public static event Action OnOpenSettingsFromMainMenu;


    private void Start()
    {        
        Instance = this;

        InitializeUI();
        SubscribeToEvents();
        HideOtherUIOnStart();
    }

    private void InitializeUI()
    {
        joinGameButton = UIHelper.GetUIElement<Button>(mainMenuUI, "JoinGameButton");
        createLobbyButton = UIHelper.GetUIElement<Button>(mainMenuUI, "CreateLobbyButton");
        optionsButton = UIHelper.GetUIElement<Button>(mainMenuUI, "OptionsButton");
        exitButton = UIHelper.GetUIElement<Button>(mainMenuUI, "ExitButton");
        editPlayerNameButton = UIHelper.GetUIElement<Button>(mainMenuUI, "EditPlayerNameButton");

    }

    private void SubscribeToEvents()
    {
        // Subcribe to button events
        joinGameButton.clicked += OnJoinGameButtonClick;
        createLobbyButton.clicked += OnCreateLobbyButtonClick;
        optionsButton.clicked += OnOptionsButtonClick;
        exitButton.clicked += OnExitButtonClick;
        editPlayerNameButton.clicked += OnEditPlayerNameButtonClick;

        // Redisplay the main menu when you leave the settings menu
        SettingsMenuUI.LeaveSettingsEvent += ShowMainMenu;
    }

    private void HideOtherUIOnStart()
    {
        // Hide all UI except for Main Menu
        UIHelper.HideUI(lobbyMenuUI, "LobbyMenu");
        UIHelper.HideUI(settingsMenuUI, "SettingsMenu");
        UIHelper.HideUI(editPlayerNamePopupUI, "EditPlayerNamePopup");
        UIHelper.HideUI(joinCodePopupUI, "JoinCodePopup");
    }

    private void OnDisable()
    {
        UnsubscribeToEvents();
    }

    // BUTTON CLICK EVENTS
    private void OnJoinGameButtonClick()
    {
        // Display Enter Join Code Popup
        UIHelper.ShowUI(joinCodePopupUI, "JoinCodePopup");
    }

    private void OnCreateLobbyButtonClick()
    {
        UIHelper.HideUI(mainMenuUI, "MainMenu");
        UIHelper.ShowUI(lobbyMenuUI, "LobbyMenu");
    }

    private void OnOptionsButtonClick()
    {
        UIHelper.HideUI(mainMenuUI, "MainMenu");
        OnOpenSettingsFromMainMenu?.Invoke();
    }

    private void OnExitButtonClick()
    {
        // Exit Game!
        Application.Quit();
    }

    private void OnEditPlayerNameButtonClick()
    {
        Debug.Log("Edit Name button clicked");
        UIHelper.ShowUI(editPlayerNamePopupUI, "EditPlayerNamePopup");
        // Display Edit Player Name Popup

        // Go to Change Player Name Script
    }

    // Re-displays the Main Menu UI when you leave the lobby menu
    // This function is a subscriber to OnBackToMainMenuEvent in LobbyMenuUI.cs
    private void ShowMainMenu()
    {
        UIHelper.HideUI(lobbyMenuUI, "LobbyMenu");
        UIHelper.ShowUI(mainMenuUI, "MainMenu");
    }

    private void UnsubscribeToEvents()
    {
        joinGameButton.clicked -= OnJoinGameButtonClick;
        createLobbyButton.clicked -= OnCreateLobbyButtonClick;
        optionsButton.clicked -= OnOptionsButtonClick;
        exitButton.clicked -= OnExitButtonClick;
        editPlayerNameButton.clicked -= OnEditPlayerNameButtonClick;
        SettingsMenuUI.LeaveSettingsEvent -= ShowMainMenu;
    }

}
