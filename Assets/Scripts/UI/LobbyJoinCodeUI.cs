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
            InputWindowUI.Show_Static("Join Code", "", "abcdefghijklmnopqrstuvxywzABCDEFGHIJKLMNOPQRSTUVXYWZ0123456789", 6,
            () => {
                // Cancel
            },
            (string joinCode) =>
            {
                LobbyHandler.Instance.JoinLobbyByCode(joinCode.ToUpper());
            });
        });

        //AuthenticateUI.Instance.OnAuthenticated += Show;
        //Hide();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Show(object sender, System.EventArgs e)
    {
        gameObject.SetActive(true);
    }
}
