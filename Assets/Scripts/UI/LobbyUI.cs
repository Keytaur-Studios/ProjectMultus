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
    [SerializeField] private Transform container;
    [SerializeField] private TextMeshProUGUI lobbyNameText;
    [SerializeField] private TextMeshProUGUI playerCountText;
    [SerializeField] private TextMeshProUGUI lobbyCodeText;
    [SerializeField] private Button leaveLobbyButton;
    [SerializeField] private Button startGameButton;


    private void Awake() {
        Instance = this;

        playerSingleTemplate.gameObject.SetActive(false);

        leaveLobbyButton.onClick.AddListener(() =>
        {
            LobbyHandler.Instance.LeaveLobby();
        });
        startGameButton.onClick.AddListener(() =>
        {
            LobbyHandler.Instance.StartGame();
        });
    }

    private void Start() {
        LobbyHandler.Instance.OnJoinedLobby += UpdateLobby_Event;
        LobbyHandler.Instance.OnJoinedLobbyUpdate += UpdateLobby_Event;
        LobbyHandler.Instance.OnLeftLobby += LobbyHandler_OnLeftLobby;
        LobbyHandler.Instance.OnKickedFromLobby += LobbyHandler_OnLeftLobby;

        Hide();
    }

    private void LobbyHandler_OnLeftLobby(object sender, System.EventArgs e) {
        ClearLobby();
        Hide();
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
            playerSingleTransform.gameObject.SetActive(true);
            LobbyPlayerSingleUI lobbyPlayerSingleUI = playerSingleTransform.GetComponent<LobbyPlayerSingleUI>();

            lobbyPlayerSingleUI.SetKickPlayerButtonVisible(
                LobbyHandler.Instance.IsLobbyHost() &&
                player.Id != AuthenticationService.Instance.PlayerId // Don't allow kick self
            );

            lobbyPlayerSingleUI.UpdatePlayer(player);
            SetStartButtonVisible(LobbyHandler.Instance.IsLobbyHost());
        }

        lobbyNameText.text = lobby.Name;
        lobbyCodeText.text = lobby.LobbyCode;
        playerCountText.text = lobby.Players.Count + "/" + lobby.MaxPlayers;

        LobbyHandler.Instance.PrintPlayers();

        Show();
    }

    private void SetStartButtonVisible(bool visible)
    {
        startGameButton.gameObject.SetActive(visible);
    }

    private void ClearLobby() {
        foreach (Transform child in container) {
            if (child == playerSingleTemplate) continue;
            Destroy(child.gameObject);
        }
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

    private void Show() {
        gameObject.SetActive(true);
    }

}