using UnityEngine;

public class setHud : MonoBehaviour
{
    public GameObject hudPrefab; // Assign in Inspector

    void Start()
    {
        // On scene start, check and apply HUD visibility
        int hudSetting = PlayerPrefs.GetInt("ShowHUD", 1); // default to ON
        hudPrefab.SetActive(hudSetting == 1);
    }

    // Optional toggle function (if you ever need to toggle again in gameplay)
    public void ShowHUD()
    {
        int hudSetting = PlayerPrefs.GetInt("ShowHUD", 1); // current value
        hudSetting = 1 - hudSetting; // toggle between 1 and 0
        PlayerPrefs.SetInt("ShowHUD", hudSetting);
        PlayerPrefs.Save();
        hudPrefab.SetActive(hudSetting == 1);
    }
}
