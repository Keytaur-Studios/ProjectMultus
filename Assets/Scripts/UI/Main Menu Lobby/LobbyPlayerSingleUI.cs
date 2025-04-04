using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine.UIElements;

public class LobbyPlayerSingleUI : MonoBehaviour {


    private string playerNameText;
    //private Button kickPlayerButton;
    private Player player;


    private void Start() {
        /*
        if (kickPlayerButton != null)
        {
            kickPlayerButton.clicked += KickPlayer;
        }
        */
    }

    /* KICK FUNCTIONALITY DOES NOT WORK YET
     
    public void SetKickPlayerButtonVisible(bool visible) {
        if (visible){
            kickPlayerButton.style.visibility = Visibility.Visible;
        }
        else
        {
            kickPlayerButton.style.visibility = Visibility.Hidden;
        }
    }

        public void setKickButton(Button kickButton)
    {
        kickPlayerButton = kickButton;
    }

    private void KickPlayer() {

        // Check if the host has the right permissions
        if (LobbyHandler.Instance.IsLobbyHost())
        {
            Debug.Log("The current player is the host and can kick players.");
        }

        if (player != null) {
            LobbyHandler.Instance.KickPlayer(player.Id);
        }
    }
    */

    public void UpdatePlayer(Player player) {
        this.player = player;
        playerNameText = player.Data[LobbyHandler.KEY_PLAYER_NAME].Value;
    }

    public string GetPlayerName() {  return playerNameText; }

    public string GetPlayerId() { return player.Id; }





}