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

    private LobbyMenuUI lobbyMenuScript;


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

        // Event triggered when you click back to main menu 
        lobbyMenuScript = lobbyMenuUI.GetComponent<LobbyMenuUI>();
        lobbyMenuScript.OnBackToMainMenuEvent += ShowMainMenu;

        // Hide all UI except for Main Menu
        HideUI(lobbyMenuUI, "LobbyMenu");
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
        HideUI(mainMenuUI, "MainMenu");

        // Display Lobby Menu UI
        ShowUI(lobbyMenuUI, "LobbyMenu");

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

    // Re-displays the Main Menu UI when you leave the lobby menu
    // This function is a subscriber to OnBackToMainMenuEvent in LobbyMenuUI.cs
    private void ShowMainMenu()
    {
        HideUI(lobbyMenuUI, "LobbyMenu");
        ShowUI(mainMenuUI, "MainMenu");
    }

    // Hides a container within a UI Document
    // UIGameObject is a GameObject with a UI Document component
    public static void HideUI(GameObject UIGameObject, string containerName)
    {
        // get container reference from UI Document
        VisualElement element = UIGameObject.GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>(containerName);
        // hide container
        element.style.visibility = Visibility.Hidden;

    }

    // Unhides a container within a UI Document
    // UIGameObject is a GameObject with a UI Document component
    public static void ShowUI(GameObject UIGameObject, string containerName)
    {
        // get container reference from UI Document
        VisualElement element = UIGameObject.GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>(containerName);
        // display container
        element.style.visibility = Visibility.Visible;

    }


}
