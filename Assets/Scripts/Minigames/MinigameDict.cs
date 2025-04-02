using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MinigameDictionary", menuName = "Game/Minigame Dictionary")]
public class MinigameDict : ScriptableObject
{
    [System.Serializable]
    public class MinigameEntry
    {
        public int id;
        public string name; // Can be any resource type
    }

    public List<MinigameEntry> minigames = new();

    private Dictionary<int, string> lookupTable;

    private void OnEnable()
    {
        lookupTable = new Dictionary<int, string>();
        foreach (var entry in minigames)
        {
            if (!lookupTable.ContainsKey(entry.id))
            {
                lookupTable.Add(entry.id, entry.name);
            }
        }
    }

    public string GetResource(int id)
    {
        return lookupTable.TryGetValue(id, out var desc) ? desc : null;
    }
}