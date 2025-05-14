using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance;
    public string gameOverSceneName = "GameOver";

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

        /*if (gameOverCanvas != null)
        { 
            gameOverCanvas.SetActive(false);
        }*/

        //DontDestroyOnLoad(gameObject);
    }

    public void ShowGameOver()
    {
        /*if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true);
        }*/
        SceneManager.LoadScene(gameOverSceneName);
    }

    public void RestartGame()
    {
        Debug.Log("RestartGame button pressed.");
        SceneManager.LoadScene("FunctionalitiesTesting");
        //Time.timeScale = 1f;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Debug.Log("QuitGame button pressed.");
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // stop play mode in editor
#endif
    }

    // Start is called before the first frame update
    void Start()
    {
        /*if (gameOverCanvas != null)
            gameOverCanvas.SetActive(false);*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
