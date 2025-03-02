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
        LobbyHandler.Instance.Authenticate(EditPlayerName.Instance.GetPlayerName());
        OnAuthenticated?.Invoke(this, EventArgs.Empty);
    }

}