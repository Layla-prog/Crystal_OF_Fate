using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSaveManager : MonoBehaviour
{
    public void SaveGame(int slot)
    {
        string screenshotPath = Application.persistentDataPath + $"/save_preview_{slot}.png";

        ScreenCapture.CaptureScreenshot($"save_preview_{slot}.png");

        PlayerPrefs.SetString($"Save{slot}_Level", SceneManager.GetActiveScene().name);
        PlayerPrefs.SetString($"Save{slot}_Preview", screenshotPath);
        PlayerPrefs.SetInt("SaveCount", Mathf.Max(PlayerPrefs.GetInt("SaveCount", 0), slot));
        PlayerPrefs.Save();

        Debug.Log("Game saved to slot " + slot + " with screenshot: " + screenshotPath);
    }
}
