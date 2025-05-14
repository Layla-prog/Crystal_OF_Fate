using UnityEngine;

using UnityEngine.SceneManagement;


public class MainMenuManager  : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
  

public int mainMenuSceneIndex = 1;
  void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            string currentScene = SceneManager.GetActiveScene().name;

            // Check if current scene is OptionsScene or SaveListScene
            if ((currentScene == "OptionsScene" || currentScene == "SaveListScene" || currentScene == "credits" || currentScene == "Instructions") &&
                SceneManager.GetActiveScene().buildIndex != mainMenuSceneIndex)
            {
                SceneManager.LoadScene(mainMenuSceneIndex);
                Debug.Log("Escape key pressed: Returning to Main Menu from " + currentScene);
            }
        }
    }


public void GoToSceneByIndex(int sceneIndex)
    {
        
        SceneManager.LoadScene(sceneIndex);
        Debug.Log("ButtonPressed");
    }



// Are You Sure - Quit Panel Pop Up
		public void AreYouSure(){
			//exitMenu.SetActive(true);
			//if(extrasMenu) extrasMenu.SetActive(false);
			//DisablePlayCampaign();
		}

public void QuitGame(){
			#if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
			#else
				Application.Quit();
			#endif
		}


 //public GameObject SampleScene;
   // public GameObject SaveListScene;

    //public void ShowSavedGamesMenu()
    //{
      //  SampleScene.SetActive(false);
        //SaveListScene.SetActive(true);
    //}

    //public void ShowMainMenu()
    //{
      //  SampleScene.SetActive(true);
        //SaveListScene.SetActive(false);
    //}

}