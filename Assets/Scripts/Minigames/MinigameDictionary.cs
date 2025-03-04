using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MinigameDictionary", menuName = "Game/Minigame Dictionary")]
public class MinigameDictionary : MonoBehaviour
{
    [System.Serializable]
    public class MinigameEntry
    {
        public string name;
        public int gameId;
        public int categoryId;
    }

    public List<MinigameEntry> minigames = new();
}
