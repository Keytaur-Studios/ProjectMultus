using System;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class LobbyMenuUI : MonoBehaviour
{
    public static LobbyMenuUI Instance { get; private set; }

    [SerializeField] private string newSceneName; // scene that is loaded when you click Start Game
    [SerializeField] private Transform playerList;
    [SerializeField] private Transform playerSingleTemplate;

    public GameObject lobbyMenuUI;
    public GameObject mainMenuUI;

    private Button backToMainMenuButton;
    private Button startGameButton;
    private Button editPlayerNameButton;
    private Label lobbyNameLabel;
    private Label lobbyCodeLabel;

    private VisualElement rootElement;
    private VisualElement playerListVisualElement;


    private void Awake()
    {

        Instance = this;

        rootElement = GetComponent<UIDocument>().rootVisualElement;
        playerListVisualElement = rootElement.Q<VisualElement>("PlayerList");

        // Initialize buttons
        backToMainMenuButton = rootElement.Q<Button>("BackToMainMenuButton");
        startGameButton = rootElement.Q<Button>("StartGameButton");
        editPlayerNameButton = rootElement.Q<Button>("EditPlayerNameButton");
        lobbyNameLabel = rootElement.Q<Label>("LobbyName");
        lobbyCodeLabel = rootElement.Q<Label>("JoinCode");

        // Subscribe to button events
        backToMainMenuButton.clicked += OnBackToMainMenuButtonClick;
        startGameButton.clicked += OnStartGameButtonClick;
        editPlayerNameButton.clicked += OnEditPlayerNameButtonClick;

        LobbyHandler.Instance.OnJoinedLobby += UpdateLobby_Event;
        LobbyHandler.Instance.OnJoinedLobby += ShowLobbyMenu;

        LobbyHandler.Instance.OnJoinedLobbyUpdate += UpdateLobby_Event;
        LobbyHandler.Instance.OnLeftLobby += LobbyHandler_OnLeftLobby;

        LobbyHandler.Instance.OnKickedFromLobby += LobbyHandler_OnLeftLobby;

        //AddPlayer(); testing
    }

    private void OnBackToMainMenuButtonClick()
    {
        Debug.Log("backbutton click");
        MainMenuUI.ShowUI(mainMenuUI, "MainMenu");
        MainMenuUI.HideUI(lobbyMenuUI, "LobbyMenu");

        rootElement.style.visibility = Visibility.Hidden; // make sure lobby menu is hidden

        LobbyHandler.Instance.LeaveLobby();
    }
    private void OnStartGameButtonClick()
    {
        Debug.Log("start game button click");
        LobbyHandler.Instance.StartGame(newSceneName);
    }
    private void OnEditPlayerNameButtonClick()
    {
        Debug.Log("edit name button click");
    }


    private void ShowLobbyMenu(object sender, LobbyHandler.LobbyEventArgs e)
    {
        MainMenuUI.HideUI(mainMenuUI, "MainMenu");
        MainMenuUI.ShowUI(lobbyMenuUI, "LobbyMenu");

    }


    private void LobbyHandler_OnLeftLobby(object sender, System.EventArgs e)
    {
        startGameButton.style.visibility = Visibility.Hidden;
        MainMenuUI.HideUI(lobbyMenuUI, "LobbyMenu");
        MainMenuUI.ShowUI(mainMenuUI, "MainMenu");
        ClearLobby();
    }

    private void UpdateLobby_Event(object sender, LobbyHandler.LobbyEventArgs e)
    {
        UpdateLobby();
    }

    private void UpdateLobby()
    {
        UpdateLobby(LobbyHandler.Instance.GetJoinedLobby());
    }

    private void UpdateLobby(Lobby lobby)
    {
        ClearLobby();

        foreach (Player player in lobby.Players)
        {
            // add a PlayerSingleTemplate to the PlayerList
            Transform playerSingleTransform = Instantiate(playerSingleTemplate, playerList);

            playerSingleTransform.gameObject.SetActive(true);


            // Add a player into the Lobby visual modal
            VisualElement playerSingleVE = AddPlayer();

                        
            LobbyPlayerSingleUI lobbyPlayerSingleUI = playerSingleTransform.GetComponent<LobbyPlayerSingleUI>();
            lobbyPlayerSingleUI.UpdatePlayer(player);
            lobbyPlayerSingleUI.setKickButton(playerSingleVE.Q<Button>());
            playerSingleVE.name = lobbyPlayerSingleUI.GetPlayerId();


            playerSingleVE.Q<Label>().text = lobbyPlayerSingleUI.GetPlayerName();

            // Set the Kick button visibility for the lobby host
            lobbyPlayerSingleUI.SetKickPlayerButtonVisible( 
                LobbyHandler.Instance.IsLobbyHost() &&
                player.Id != AuthenticationService.Instance.PlayerId // Don't allow kick self
            );

            SetStartButtonVisible(LobbyHandler.Instance.IsLobbyHost());
        }

        lobbyNameLabel.text = lobby.Name; 
        lobbyCodeLabel.text = lobby.LobbyCode; 

        LobbyHandler.Instance.PrintPlayers();
    }

    private VisualElement AddPlayer()
    {
        VisualElement playerElement = new VisualElement();
        playerElement.AddToClassList("playerContainer");

        VisualElement playerIcon = new VisualElement();
        playerIcon.AddToClassList("playerIcon");

        Label playerName = new Label();
        playerName.AddToClassList("playerNameText");

        Button kickButton = new Button();
        kickButton.name = "KickPlayerButton";
        kickButton.AddToClassList("kickPlayerButton");
        kickButton.AddToClassList("button");

        playerElement.Add(playerIcon);
        playerElement.Add(playerName);
        playerElement.Add(kickButton);

        playerListVisualElement.Add(playerElement);

        return playerElement;
    }



    private void SetStartButtonVisible(bool visible)
    {
        Debug.Log("setting start button visible");
        if (visible)
        {
            startGameButton.style.visibility = Visibility.Visible;
        }
        else
        {
            startGameButton.style.visibility = Visibility.Hidden;
        }
    }

    private void ClearLobby()
    {            
            foreach (Transform child in playerList)
            {
                if (child == playerSingleTemplate) 
                {
                    continue; 
                }
                Destroy(child.gameObject); // remove playerSingle from playerList
            }
            playerListVisualElement.Clear();
    }

    public void UnsubscribeUI()
    {
        LobbyHandler.Instance.OnJoinedLobby -= UpdateLobby_Event;
        LobbyHandler.Instance.OnJoinedLobbyUpdate -= UpdateLobby_Event;
        LobbyHandler.Instance.OnLeftLobby -= LobbyHandler_OnLeftLobby;
        LobbyHandler.Instance.OnKickedFromLobby -= LobbyHandler_OnLeftLobby;
    }

}
