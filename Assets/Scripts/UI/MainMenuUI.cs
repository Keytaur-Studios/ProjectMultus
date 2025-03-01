using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuUI : MonoBehaviour
{
    public GameObject mainMenuUI;
    public GameObject lobbyMenuUI;
    public GameObject joinGamePopupUI;
    public GameObject editPlayerNamePopupUI;
    public GameObject settingsMenuUI;

    private Button joinGameButton, createLobbyButton, optionsButton, exitButton, editPlayerNameButton;


    private void Awake()
    {
        // Initialize buttons
        joinGameButton = mainMenuUI.GetComponent<UIDocument>().rootVisualElement.Q<Button>("JoinGameButton");
        createLobbyButton = mainMenuUI.GetComponent<UIDocument>().rootVisualElement.Q<Button>("CreateLobbyButton");
        optionsButton = mainMenuUI.GetComponent<UIDocument>().rootVisualElement.Q<Button>("OptionsButton");
        exitButton = mainMenuUI.GetComponent<UIDocument>().rootVisualElement.Q<Button>("ExitButton");
        editPlayerNameButton = mainMenuUI.GetComponent<UIDocument>().rootVisualElement.Q<Button>("EditPlayerNameButton");



        // Subcribe to button events
        joinGameButton.clicked += OnJoinGameButtonClick;
        createLobbyButton.clicked += OnCreateLobbyButtonClick;
        optionsButton.clicked += OnOptionsButtonClick; 
        exitButton.clicked += OnExitButtonClick;
        editPlayerNameButton.clicked += OnEditPlayerNameButtonClick;

    }

    private void OnDisable()
    {
        // Unsubscribe to button events
        joinGameButton.clicked -= OnJoinGameButtonClick;
        createLobbyButton.clicked -= OnCreateLobbyButtonClick;
        optionsButton.clicked -= OnOptionsButtonClick;
        exitButton.clicked -= OnExitButtonClick;
        editPlayerNameButton.clicked -= OnEditPlayerNameButtonClick;
    }

    // BUTTON CLICK EVENTS
    private void OnJoinGameButtonClick()
    {
        Debug.Log("Join button clicked");

        // Display Enter Join Code Popup
    }

    private void OnCreateLobbyButtonClick()
    {
        Debug.Log("Create Lobby button clicked");

        // Hide Main Menu UI

        // Display Lobby Menu UI

        // Awake Lobby Menu script?

    }

    private void OnOptionsButtonClick()
    {
        Debug.Log("Options button clicked");

        // Hide Main Menu UI

        // Open Settings UI

    }

    private void OnExitButtonClick()
    {
        Debug.Log("Exit button clicked");

        // Exit Game!

    }

    private void OnEditPlayerNameButtonClick()
    {
        Debug.Log("Edit Name button clicked");

        // Display Edit Player Name Popup

        // Go to Change Player Name Script
    }





}
