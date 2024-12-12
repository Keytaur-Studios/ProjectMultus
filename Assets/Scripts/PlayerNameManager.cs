using UnityEngine;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using Unity.Netcode;
using System;

public class PlayerNameManager : MonoBehaviour
{
    public static PlayerNameManager Instance { get; private set; }

    public string thisPlayerId;
    public ulong thisPlayerId2;
    public Dictionary<string, string> playerNames = new Dictionary<string, string>();
    public Dictionary<ulong, string> playerNamesByClientId = new Dictionary<ulong, string>();

    //public NetworkList<ulong, string> playerNamesByClientNetwork = 

    private void Start()
    {
        Instance = this;
    }

    public void UpdateNames(Lobby lobby)
    {
        foreach (Player p in lobby.Players)
        {
            playerNames.Add(p.Id, p.Data["PlayerName"].Value);
        }
    }

    public void UpdateIDNames()
    {
        
    }
}
