using UnityEngine;
using System;
using UnityEngine.UIElements;

public class LobbyJoinCodeUI : MonoBehaviour
{
    public static LobbyJoinCodeUI Instance { get; private set; }

    public GameObject mainMenuUI;
    public GameObject joinCodePopupUI;
    public GameObject lobbyMenuUI;

    private Button goButton;
    private Button cancelButton;
    private TextField joinCodeInput;

    private LobbyHandler lobbyHandler;


    private void Start()
    {
        Instance = this;

        lobbyHandler = GameObject.Find("NetworkManager").GetComponent<LobbyHandler>();

        // Initialize UI Elements
        goButton = UIHelper.GetUIElement<Button>(joinCodePopupUI, "GoButton");
        cancelButton = UIHelper.GetUIElement<Button>(joinCodePopupUI, "CancelButton");
        joinCodeInput = UIHelper.GetUIElement<TextField>(joinCodePopupUI, "JoinCodeInput");

        // Subscribe to events
        goButton.clicked += OnGoButtonClick;
        cancelButton.clicked += OnCancelButtonClick;
    }

    
    private void OnGoButtonClick()
    {
        // attempt to join a lobby with user input
        string joinCode = joinCodeInput.value;
        LobbyHandler.Instance.JoinLobbyByCode(joinCode.ToUpper());

        joinCodeInput.value = ""; // clear value
        UIHelper.HideUI(joinCodePopupUI, "JoinCodePopup");

    }

    private void OnCancelButtonClick()
    {
        UIHelper.HideUI(joinCodePopupUI, "JoinCodePopup");
    }
}

