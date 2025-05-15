using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance;
    public string gameOverSceneName = "GameOver";
    public string restartSceneName = "Level1";

    //public GameObject gameOverCanvas;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            if (SceneManager.GetActiveScene().name != gameOverSceneName)
            {
                Destroy(gameObject);
            }
        }
    }

    public void ShowGameOver()
    {
        SceneManager.LoadScene(gameOverSceneName);
    }

    public void RestartGame()
    {
        Debug.Log("RestartGame button pressed.");
        SceneManager.LoadScene(restartSceneName);
    
    }

    public void QuitGame()
    {
        Debug.Log("QuitGame button pressed.");
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // stop play mode in editor
#endif
    }
}
