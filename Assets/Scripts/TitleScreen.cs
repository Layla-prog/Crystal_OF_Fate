using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public float delayBeforeLoad = 3f;

    public string nextScene = "MainMenu";

    // Start is called before the first frame update
    void Start()
    {
        //Start the coroutine that waits before loading the next scene
        StartCoroutine(LoadNextSceneAfterDelay());   
    }

    //Coroutine - waits a few seconds, then loads the next scene
    private System.Collections.IEnumerator LoadNextSceneAfterDelay()
    {
        //wait for the specified number of seconds 
        yield return new WaitForSeconds(delayBeforeLoad);
        
        //Load the next scene by Name
        SceneManager.LoadScene(nextScene);
    }

}
