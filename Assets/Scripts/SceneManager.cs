using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance { get; private set; }

    [SerializeField] private string currentScene;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private SceneInfo sceneInfo;

    private void Awake()
    {
        Instance = this;
    }

    private void ServerStartDebug_Event(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        Debug.Log("All clients connected scene starting.");
    }

    private void OnSceneLoaded(ulong clientId, string sceneName, LoadSceneMode loadSceneMode)
    {
        sceneInfo = GameObject.Find("SceneInfo").GetComponent<SceneInfo>();
        if (NetworkManager.Singleton.IsServer)
        {
            Debug.Log($"Scene '{sceneName}' loaded for client {clientId}");

            // Spawn the player for the client that finished loading
            GameObject playerInstance = Instantiate(playerPrefab, sceneInfo.SpawnPoint(), Quaternion.Euler(0, 0, 0));

            // Attach the NetworkObject to the client
            playerInstance.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);

            Debug.Log($"Player spawned for client {clientId} in scene '{sceneName}'");
        }
    }

    public void LoadScene(string sceneToLoad)
    {
        if (NetworkManager.Singleton.IsHost && !string.IsNullOrEmpty(sceneToLoad))
        {
            NetworkManager.Singleton.
                SceneManager.OnLoadEventCompleted += ServerStartDebug_Event;

            NetworkManager.Singleton.SceneManager.OnLoadComplete += OnSceneLoaded;

            var status = NetworkManager.Singleton.SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
            if (status != SceneEventProgressStatus.Started)
            {
                Debug.LogWarning($"Failed to load {sceneToLoad} " +
                      $"with a {nameof(SceneEventProgressStatus)}: {status}");
            }
        }
    }

}
