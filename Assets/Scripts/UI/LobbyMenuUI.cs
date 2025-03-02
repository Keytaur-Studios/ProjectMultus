using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class LobbyMenuUI : MonoBehaviour
{
    
    private Button backToMainMenuButton;
    private Button startGameButton;
    private Button editPlayerNameButton;

    private VisualElement rootElement;
    private VisualElement playerList;

    public event Action OnBackToMainMenuEvent;


    private void Awake()
    {
        rootElement = GetComponent<UIDocument>().rootVisualElement;
        playerList = rootElement.Q<VisualElement>("PlayerList");

        // Initialize buttons
        backToMainMenuButton = rootElement.Q<Button>("BackToMainMenuButton");
        startGameButton = rootElement.Q<Button>("StartGameButton");
        editPlayerNameButton = rootElement.Q<Button>("EditPlayerNameButton");

        // Subscribe to button events
        backToMainMenuButton.clicked += OnBackToMainMenuButtonClick;
        startGameButton.clicked += OnStartGameButtonClick;
        editPlayerNameButton.clicked += OnEditPlayerNameButtonClick;

        AddPlayer();
    }

    private void AddPlayer()
    {
        VisualElement player = new VisualElement();
        player.AddToClassList("playerContainer");
        
        VisualElement playerIcon = new VisualElement();
        playerIcon.AddToClassList("playerIcon");
        
        Label playerName = new Label();
        playerName.AddToClassList("playerNameText");
        playerName.AddToClassList("playerNameText");
        playerName.AddToClassList("playerNameText");

        Button kickButton = new Button();
        kickButton.name = "KickPlayerButton";
        kickButton.AddToClassList("kickPlayerButton");
        kickButton.AddToClassList("button");
        
        player.Add(playerIcon);
        player.Add(playerName);
        player.Add(kickButton);

        playerList.Add(player);
    }


    private void OnBackToMainMenuButtonClick()
    {
        Debug.Log("back button click");
        OnBackToMainMenuEvent?.Invoke();
    }
    private void OnStartGameButtonClick()
    {
        Debug.Log("start game button click");
    }
    private void OnEditPlayerNameButtonClick()
    {
        Debug.Log("edit name button click");
    }
}
