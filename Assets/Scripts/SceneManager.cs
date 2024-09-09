using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance { get; private set; }

    [SerializeField] private string currentScene;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {

    }

    private void ServerStartDebug_Event(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        Debug.Log("All clients connected scene starting.");
    }

    public void LoadScene(string sceneToLoad)
    {
        if (NetworkManager.Singleton.IsHost && !string.IsNullOrEmpty(sceneToLoad))
        {
            NetworkManager.Singleton.
                SceneManager.OnLoadEventCompleted += ServerStartDebug_Event;

            var status = NetworkManager.Singleton.SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
            if (status != SceneEventProgressStatus.Started)
            {
                Debug.LogWarning($"Failed to load {sceneToLoad} " +
                      $"with a {nameof(SceneEventProgressStatus)}: {status}");
            }
        }
    }

}
