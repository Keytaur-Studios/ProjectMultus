using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class LobbyMenuUI : MonoBehaviour
{
    
    private Button backToMainMenuButton;
    private Button startGameButton;
    private Button editPlayerNameButton;

    public event Action OnBackToMainMenuEvent;


    private void Awake()
    {
        // Initialize buttons
        backToMainMenuButton = GetComponent<UIDocument>().rootVisualElement.Q<Button>("BackToMainMenuButton");
        startGameButton = GetComponent<UIDocument>().rootVisualElement.Q<Button>("StartGameButton");
        editPlayerNameButton = GetComponent<UIDocument>().rootVisualElement.Q<Button>("EditPlayerNameButton");

        // Subscribe to button events
        backToMainMenuButton.clicked += OnBackToMainMenuButtonClick;
        startGameButton.clicked += OnStartGameButtonClick;
        editPlayerNameButton.clicked += OnEditPlayerNameButtonClick;

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
