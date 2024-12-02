using System.Collections.Generic;
using UnityEngine;

public class SceneInfo : MonoBehaviour
{
    public string Name;

    [Header("Player Spawn Info")]
    public List<Vector3> spawnPoints;
    public int playersSpawned = 0;

    public Vector3 SpawnPlayer()
    {
        int idx = Random.Range(0, spawnPoints.Count);
        Vector3 spawnPoint = spawnPoints[idx];
        spawnPoints.RemoveAt(idx);
        playersSpawned++;
        return spawnPoint;
    }
}
