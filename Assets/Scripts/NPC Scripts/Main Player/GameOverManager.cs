using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance;
    public GameObject gameOverCanvas;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (gameOverCanvas != null)
        { 
            gameOverCanvas.SetActive(false);
        }

        //DontDestroyOnLoad(gameObject);
    }

    public void ShowGameOver()
    {
        //Time.timeScale = 0f;

        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true);
            //Time.timeScale = 0f;
        }
    }

    public void RestartGame()
    {
        //Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // stop play mode in editor
#endif
    }

    // Start is called before the first frame update
    void Start()
    {
        if (gameOverCanvas != null)
            gameOverCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
