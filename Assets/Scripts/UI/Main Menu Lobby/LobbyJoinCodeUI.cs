using UnityEngine;
using System;
using UnityEngine.UIElements;

public class LobbyJoinCodeUI : MonoBehaviour
{
    public static LobbyJoinCodeUI Instance { get; private set; }

    [SerializeField] GameObject mainMenuUI;
    [SerializeField] GameObject joinCodePopupUI;
    [SerializeField] GameObject lobbyMenuUI;

    private Button goButton;
    private Button cancelButton;
    private TextField joinCodeInput;


    private void Start()
    {
        Instance = this;

        // Initialize UI Elements
        goButton = UIHelper.GetUIElement<Button>(joinCodePopupUI, "GoButton");
        cancelButton = UIHelper.GetUIElement<Button>(joinCodePopupUI, "CancelButton");
        joinCodeInput = UIHelper.GetUIElement<TextField>(joinCodePopupUI, "JoinCodeInput");


        // Subscribe to events
        goButton.clicked += OnGoButtonClick;
        cancelButton.clicked += OnCancelButtonClick;

        Debug.Log("join init");
    }


    
    private void OnGoButtonClick()
    {
        Debug.Log("join click");
        // attempt to join a lobby with user input
        string joinCode = joinCodeInput.value;
        LobbyHandler.Instance.JoinLobbyByCode(joinCode.ToUpper());

        UIHelper.HideUI(joinCodePopupUI, "JoinCodePopup");

        joinCodeInput.value = ""; // clear value

    }

    private void OnCancelButtonClick()
    {
        Debug.Log("join click");

        UIHelper.HideUI(joinCodePopupUI, "JoinCodePopup");
        joinCodeInput.value = ""; // clear value
    }
}

