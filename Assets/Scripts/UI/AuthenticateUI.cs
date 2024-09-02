using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AuthenticateUI : MonoBehaviour
{
    public static AuthenticateUI Instance { get; private set; }

    public event EventHandler OnAuthenticated;

    [SerializeField] private Button authenticateButton;

    private void Awake()
    {
        Instance = this;
        authenticateButton.onClick.AddListener(() =>
        {
            LobbyHandler.Instance.Authenticate(EditPlayerName.Instance.GetPlayerName());
            Hide();
            OnAuthenticated?.Invoke(this, EventArgs.Empty);
        });
    }

    private void Hide()
    {
        gameObject.transform.localScale = Vector3.zero;
        //gameObject.SetActive(false);
    }
}