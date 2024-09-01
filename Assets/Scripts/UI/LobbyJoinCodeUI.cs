using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class LobbyJoinCodeUI : MonoBehaviour
{
    [SerializeField] private Button joinButton;

    private void Awake()
    {
        joinButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            InputWindowUI.Show_Static("Join Code", "", "abcdefghijklmnopqrstuvxywzABCDEFGHIJKLMNOPQRSTUVXYWZ", 6,
            () => {
                // Cancel
            },
            (string joinCode) =>
            {
                LobbyHandler.Instance.JoinLobbyByCode(joinCode.ToUpper());
            });
        });
    }
}
