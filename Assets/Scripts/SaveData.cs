using UnityEngine;

[System.Serializable]
public class SaveData
{
    public long timestamp;            // When the save was made
    public int slotNumber;            // The save slot (assigned at runtime, not saved)
    public int levelIndex;            // Build index or custom index
    public string levelName;           // Name of the level
    public Vector3 playerPosition;    // Position of player

    // Add other fields as needed (health, inventory, etc.)
}
