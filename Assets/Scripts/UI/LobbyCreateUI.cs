using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class LobbyCreateUI : MonoBehaviour {


    public static LobbyCreateUI Instance { get; private set; }

    public GameObject mainMenuUI;

    private Button createLobbyButton;

    

    private void Start() {
        Instance = this;

        createLobbyButton = mainMenuUI.GetComponent<UIDocument>().rootVisualElement.Q<Button>("CreateLobbyButton");
        createLobbyButton.clicked += onCreateLobbyButtonClick;

        LobbyHandler.Instance.OnJoinedLobby += HideOnEvent;
        LobbyHandler.Instance.OnLeftLobby += ShowOnEvent;

    }

    private void onCreateLobbyButtonClick()
    {
        LobbyHandler.Instance.CreateLobby(
                EditPlayerName.Instance.GetPlayerName() + "'s Game"
            );
    }

    private void Hide()
    {
        gameObject.transform.localScale = Vector3.zero;
    }

    private void ShowOnEvent(object sender, System.EventArgs e)
    {
        gameObject.transform.localScale = Vector3.one;
    }

    private void HideOnEvent(object sender, System.EventArgs e)
    {
        gameObject.transform.localScale = Vector3.zero;
    }

}