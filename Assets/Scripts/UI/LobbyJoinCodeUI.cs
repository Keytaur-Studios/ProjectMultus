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

        goButton = joinCodePopupUI.GetComponent<UIDocument>().rootVisualElement.Q<Button>("GoButton");
        cancelButton = joinCodePopupUI.GetComponent<UIDocument>().rootVisualElement.Q<Button>("CancelButton");
        joinCodeInput = joinCodePopupUI.GetComponent<UIDocument>().rootVisualElement.Q<TextField>("JoinCodeInput");

        goButton.clicked += OnGoButtonClick;
        cancelButton.clicked += OnCancelButtonClick;
    }

    
    private void OnGoButtonClick()
    {
        // attempt to join a lobby with user input
        string joinCode = joinCodeInput.value;
        LobbyHandler.Instance.JoinLobbyByCode(joinCode.ToUpper());

        joinCodeInput.value = ""; // clear value
        MainMenuUI.HideUI(joinCodePopupUI, "JoinCodePopup");

    }

    private void OnCancelButtonClick()
    {
        MainMenuUI.HideUI(joinCodePopupUI, "JoinCodePopup");
    }
}

