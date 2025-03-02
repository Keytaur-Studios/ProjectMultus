using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AuthenticateUI : MonoBehaviour
{
    public static AuthenticateUI Instance { get; private set; }

    public event EventHandler OnAuthenticated;

    private void Awake()
    {
        Instance = this;
        if (LobbyHandler.Instance == null)
            Debug.Log("LobbyHandler.Instance is null");
        if (EditPlayerName.Instance == null)
            Debug.Log("EditPlayerName.Instance is null");
        if (EditPlayerName.Instance.GetPlayerName() == null)
            Debug.Log("name is null yet");
        LobbyHandler.Instance.Authenticate(EditPlayerName.Instance.GetPlayerName());
        OnAuthenticated?.Invoke(this, EventArgs.Empty);
    }

}