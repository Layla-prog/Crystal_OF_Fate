using System.IO;
using UnityEngine;

public class DummySaveCreator : MonoBehaviour
{
    public int numberOfDummies = 2; // How many dummy saves to create
    
    void Start()
    {
        for (int i = 1; i <= numberOfDummies; i++)
        {
            SaveData dummy = new SaveData
            {
                timestamp = System.DateTime.Now.Ticks + i, // make them different
                slotNumber = i
            };

            string path = Application.persistentDataPath + "/Save" + i + ".json";
            File.WriteAllText(path, JsonUtility.ToJson(dummy));

            Debug.Log($"Dummy save created: {path}");

            
        }

        Debug.Log("All dummy saves generated.");
    }
}
