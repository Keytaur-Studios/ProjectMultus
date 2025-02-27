using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResourceDictionary", menuName = "Game/Resource Dictionary")]
public class ResourceDictionary : ScriptableObject
{
    [System.Serializable]
    public class ResourceEntry
    {
        public int id;
        public GameObject prefab; // Can be any resource type
    }

    public List<ResourceEntry> resources = new();

    private Dictionary<int, GameObject> lookupTable;

    private void OnEnable()
    {
        lookupTable = new Dictionary<int, GameObject>();
        foreach (var entry in resources)
        {
            if (!lookupTable.ContainsKey(entry.id))
            {
                lookupTable.Add(entry.id, entry.prefab);
            }
        }
    }

    public GameObject GetResource(int id)
    {
        return lookupTable.TryGetValue(id, out var resource) ? resource : null;
    }
}