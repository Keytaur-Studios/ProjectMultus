using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button serverBtn;
    [SerializeField] private Button hostBtn;
    [SerializeField] private Button clientBtn;

    [SerializeField] private Button createLobbyBtn;
    [SerializeField] private Button listLobbyBtn;
    [SerializeField] private Button joinLobbyBtn;
    [SerializeField] private Button leaveLobbyBtn;
    [SerializeField] private Button deleteLobbyBtn;

    [SerializeField] private Button createRelayBtn;
    [SerializeField] private Button joinRelayBtn;

    [SerializeField] private Button startGameBtn;

    [SerializeField]private Button printPlayersBtn;

    [SerializeField] private string lobbyCode;
    [SerializeField] private string relayCode;


    private void Awake()
    {
        serverBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartServer();
        });
        hostBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
        });
        clientBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
        });
        createLobbyBtn.onClick.AddListener(() =>
        {
            GameObject.Find("Lobby").GetComponent<LobbyHandler>().CreateLobby();
        });
        listLobbyBtn.onClick.AddListener(() =>
        {
            GameObject.Find("Lobby").GetComponent<LobbyHandler>().ListLobbies();
        });
        joinLobbyBtn.onClick.AddListener(() =>
        {
            GameObject.Find("Lobby").GetComponent<LobbyHandler>().JoinLobbyByCode(lobbyCode);
        });
        leaveLobbyBtn.onClick.AddListener(() =>
        {
            GameObject.Find("Lobby").GetComponent<LobbyHandler>().LeaveLobby();
        });
        deleteLobbyBtn.onClick.AddListener(() =>
        {
            GameObject.Find("Lobby").GetComponent<LobbyHandler>().DeleteLobby();
        });
        printPlayersBtn.onClick.AddListener(() =>
        {
            GameObject.Find("Lobby").GetComponent<LobbyHandler>().PrintPlayers();
        });
        createRelayBtn.onClick.AddListener(() =>
        {
            GameObject.Find("Relay").GetComponent<RelayHandler>().CreateRelay();
        });
        joinRelayBtn.onClick.AddListener(() =>
        {
            GameObject.Find("Relay").GetComponent<RelayHandler>().JoinRelay(relayCode);
        });
        startGameBtn.onClick.AddListener(() =>
        {
            LobbyHandler.Instance.StartGame();
        });
    }
}
