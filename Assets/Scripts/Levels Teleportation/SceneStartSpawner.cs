using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneStartSpawner : MonoBehaviour
{
    public GameObject player;
    public GameObject eedie;
    public GameObject kate;
    public GameObject miroku;

    // Start is called before the first frame update
    void Start()
    {
        if (TeleportData.Instance != null)
        {
            if (player) player.transform.position = TeleportData.Instance.playerSpawn;
            if (eedie) eedie.transform.position = TeleportData.Instance.eedieSpawn;
            if (kate) kate.transform.position = TeleportData.Instance.kateSpawn;
            if (miroku) miroku.transform.position = TeleportData.Instance.mirokuSpawn;
        }
    }
}
