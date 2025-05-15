using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportTrigger : MonoBehaviour
{
    public string nextSceneName;

    public Vector3 playerSpawn;
    public Vector3 eedieSpawn;
    public Vector3 kateSpawn;
    public Vector3 mirokuSpawn;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TeleportData.Instance.playerSpawn = playerSpawn;
            TeleportData.Instance.eedieSpawn = eedieSpawn;
            TeleportData.Instance.kateSpawn = kateSpawn;
            TeleportData.Instance.mirokuSpawn = mirokuSpawn;

            SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single); // Replace current scene
        }
    }
}
