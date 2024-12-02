using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SceneInfo : MonoBehaviour
{
    public string Name;

    [Header("Player Spawn Info")]
    public List<Vector3> spawnPoints;
    public int playersSpawned = 0;

    public Vector3 SpawnPoint()
    {
        int idx = Random.Range(0, spawnPoints.Count);
        Debug.Log($"Random number {idx} out of {spawnPoints.Count} spawnpoints");
        Vector3 spawnPoint = spawnPoints[idx];
        spawnPoints.RemoveAt(idx);
        playersSpawned++;
        return spawnPoint;
    }
}
