using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour {


    public static LobbyUI Instance { get; private set; }


    [SerializeField] private Transform playerSingleTemplate;
    // private VisualElement playerSingleTemplate;
    [SerializeField] private Transform container;
    // private VisualElement PlayerList;
    [SerializeField] private TextMeshProUGUI lobbyNameText; 
    // private Label LobbyName;
    [SerializeField] private TextMeshProUGUI playerCountText; // delete
    
    [SerializeField] private TextMeshProUGUI lobbyCodeText;
    // private Label lobbyCodeText;
    [SerializeField] private Button leaveLobbyButton;
    // private Button backToMainMenuButton;
    [SerializeField] private Button startGameButton;
    // private Button startGameButton;
    [SerializeField] private string newSceneName;

    private void Awake() {
        Instance = this;

        // initialize UI elements

        playerSingleTemplate.gameObject.SetActive(false); // delete

        leaveLobbyButton.onClick.AddListener(() =>
        {
            LobbyHandler.Instance.LeaveLobby();
        });
        startGameButton.onClick.AddListener(() =>
        {
            LobbyHandler.Instance.StartGame(newSceneName);
            //LobbyHandler.Instance.StartGame();
        });
    }

    private void Start() {
        LobbyHandler.Instance.OnJoinedLobby += UpdateLobby_Event;
        LobbyHandler.Instance.OnJoinedLobbyUpdate += UpdateLobby_Event;
        LobbyHandler.Instance.OnLeftLobby += LobbyHandler_OnLeftLobby;
        LobbyHandler.Instance.OnKickedFromLobby += LobbyHandler_OnLeftLobby;

        Hide(); // delete
    }

    private void LobbyHandler_OnLeftLobby(object sender, System.EventArgs e) {
        ClearLobby();
        Hide(); // delete
    }

    private void UpdateLobby_Event(object sender, LobbyHandler.LobbyEventArgs e) {
        UpdateLobby();
    }

    private void UpdateLobby() {
        UpdateLobby(LobbyHandler.Instance.GetJoinedLobby());
    }

    private void UpdateLobby(Lobby lobby) {
        ClearLobby();

        foreach (Player player in lobby.Players) {
            
            Transform playerSingleTransform = Instantiate(playerSingleTemplate, container); 
            
            // add a PlayerSingleTemplate to the PlayerList
            
            playerSingleTransform.gameObject.SetActive(true);  // ??
            LobbyPlayerSingleUI lobbyPlayerSingleUI = playerSingleTransform.GetComponent<LobbyPlayerSingleUI>();  // ??
            // lobbyPlayerSingleUI.kickButton = playerSingleTransform.getUIbutton
            /*
            lobbyPlayerSingleUI.SetKickPlayerButtonVisible( // update method 
                LobbyHandler.Instance.IsLobbyHost() &&
                player.Id != AuthenticationService.Instance.PlayerId // Don't allow kick self
            );
            */

            lobbyPlayerSingleUI.UpdatePlayer(player);
            SetStartButtonVisible(LobbyHandler.Instance.IsLobbyHost());
        }

        lobbyNameText.text = lobby.Name; // lobbyNameText.text = lobby.Name;
        lobbyCodeText.text = lobby.LobbyCode; // joinCode.text = lobby.LobbyCode;
        playerCountText.text = lobby.Players.Count + "/" + lobby.MaxPlayers; // delete

        LobbyHandler.Instance.PrintPlayers();

        Show(); // ??
    }

    private void SetStartButtonVisible(bool visible) 
    {
        //startGameButton.style.visibility = Visibility.Hidden
    }

    private void ClearLobby() {
        if (this.gameObject.activeInHierarchy == true)
        foreach (Transform child in container) {
            if (child == playerSingleTemplate) continue;
            Destroy(child.gameObject); // remove playerSingle from playerList
        }
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

    public void UnsubscribeUI()
    {
        LobbyHandler.Instance.OnJoinedLobby -= UpdateLobby_Event;
        LobbyHandler.Instance.OnJoinedLobbyUpdate -= UpdateLobby_Event;
        LobbyHandler.Instance.OnLeftLobby -= LobbyHandler_OnLeftLobby;
        LobbyHandler.Instance.OnKickedFromLobby -= LobbyHandler_OnLeftLobby;
    }

    private void Show() {
        gameObject.SetActive(true);
    }

}