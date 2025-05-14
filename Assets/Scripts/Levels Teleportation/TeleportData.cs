using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportData : MonoBehaviour
{
    public static TeleportData Instance;

    public Vector3 playerSpawn;
    public Vector3 eedieSpawn;
    public Vector3 kateSpawn;
    public Vector3 mirokuSpawn;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
