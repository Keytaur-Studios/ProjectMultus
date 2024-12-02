using System.Collections.Generic;
using UnityEngine;

public class TestSceneSwitch : SceneInfo
{
    private void Awake()
    {
        spawnPoints = new List<Vector3> {
            new (0, .5f, 0),
            new (-5, .5f, 0),
            new (-5, .5f, 5),
            new (0, .5f, 5),
        };
    }
}
