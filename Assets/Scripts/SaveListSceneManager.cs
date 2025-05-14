using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;

public class SaveListSceneManager : MonoBehaviour
{
    public GameObject saveSlotPrefab;
    public Transform savePanel;
    public Sprite defaultImage;
    [SerializeField] private MenuButtonController menuButtonController;    private const int maxSaves = 3;
    private List<SaveData> saveDataList = new List<SaveData>();

    private void Start()
    {
        LoadAllSaves();
        DisplaySaveSlots();
        AddNewAdventureButton();

        if (PlayerPrefs.HasKey("PlayerX"))
    {
        Vector3 loadedPos = new Vector3(
            PlayerPrefs.GetFloat("PlayerX"),
            PlayerPrefs.GetFloat("PlayerY"),
            PlayerPrefs.GetFloat("PlayerZ")
        );

        transform.position = loadedPos;
    }

    }

    void LoadAllSaves()
    {
        saveDataList.Clear();

        for (int i = 1; i <= maxSaves; i++)
        {
            string path = Application.persistentDataPath + "/Save" + i + ".json";
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                SaveData data = JsonUtility.FromJson<SaveData>(json);
                data.slotNumber = i;
                saveDataList.Add(data);
            }
        }

        // Sort by timestamp to manage overwriting later
        saveDataList.Sort((a, b) => a.timestamp.CompareTo(b.timestamp));
    }

    void DisplaySaveSlots()
    {
        foreach (var save in saveDataList)
        {
           
            GameObject slot = Instantiate(saveSlotPrefab, savePanel);
            
            MenuButton menuButton = slot.GetComponent<MenuButton>();
            if (menuButton != null)
            {
            menuButton.MenuButtonController = menuButtonController /* reference to your controller */;
            menuButton.Animator = slot.GetComponent<Animator>();
            menuButton.AnimatorFunctions = slot.GetComponent<AnimatorFunctions>();
            menuButton.ThisIndex = save.slotNumber;         
            }

            AnimatorFunctions animatorFunctions = slot.GetComponent<AnimatorFunctions>();
if (animatorFunctions != null)
{
    animatorFunctions.disableOnce = false;
    animatorFunctions.GetType()
        .GetField("menuButtonController", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
        ?.SetValue(animatorFunctions, menuButtonController);
}


            string fullPath = SceneUtility.GetScenePathByBuildIndex(5); // e.g., "Assets/Scenes/Level1.unity"
            string sceneName = Path.GetFileNameWithoutExtension(fullPath); // returns "Level1"
            save.levelName = sceneName;


            slot.GetComponentInChildren<Text>().text = $"Slot {save.slotNumber} - {save.levelName}";


            // Load image from file if you saved it, else use default
            Image img = slot.transform.Find("Ellipse").GetComponent<Image>();
            string imagePath = Application.persistentDataPath + "/Save" + save.slotNumber + ".png";
            if (File.Exists(imagePath))
            {
                byte[] imgBytes = File.ReadAllBytes(imagePath);
                Texture2D tex = new Texture2D(2, 2);
                tex.LoadImage(imgBytes);
                img.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one * 0.5f);
            }
            else
            {
                img.sprite = defaultImage;
            }

            int slotNumber = save.slotNumber;
            slot.GetComponent<Button>().onClick.AddListener(() => LoadGame(slotNumber));
                Debug.Log("slot number " + slotNumber);

        }
    }

    void AddNewAdventureButton()
    {
        GameObject newButton = Instantiate(saveSlotPrefab, savePanel);

        MenuButton menuButton = newButton.GetComponent<MenuButton>();
            if (menuButton != null)
            {
             menuButton.MenuButtonController = menuButtonController /* reference to your controller */;
             menuButton.Animator = newButton.GetComponent<Animator>();
            AnimatorFunctions animatorFunctions = newButton.GetComponent<AnimatorFunctions>();
            if (animatorFunctions != null)
            {
                animatorFunctions.disableOnce = false;
                animatorFunctions.GetType().GetField("menuButtonController", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(animatorFunctions, menuButtonController);
            }

            menuButton.ThisIndex = saveDataList.Count; 
            }

        newButton.GetComponentInChildren<Text>().text = "New Adventure";
        Image img = newButton.transform.Find("Ellipse").GetComponent<Image>();
if (img != null)
{
    img.sprite = defaultImage;
}
else
{
    Debug.LogWarning("Image not found in prefab.");
}


        newButton.GetComponent<Button>().onClick.AddListener(() => StartNewGame());
         Debug.Log("StartNewGame called");
    }

    void LoadGame(int slot)
    {
        Debug.Log("Loading Save " + slot);
        // You can load your scene and pass slot number to load specific data
        
        string path = Application.persistentDataPath + "/Save" + slot + ".json";
    if (File.Exists(path))
    {
        string json = File.ReadAllText(path);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        PlayerPrefs.SetFloat("PlayerX", data.playerPosition.x);
        PlayerPrefs.SetFloat("PlayerY", data.playerPosition.y);
        PlayerPrefs.SetFloat("PlayerZ", data.playerPosition.z);
        PlayerPrefs.SetInt("CurrentSaveSlot", slot);

        SceneManager.LoadScene(data.levelIndex); // Load the saved level
    }
    else
    {
        Debug.LogError("Save file not found.");
    }

        //SceneManager.LoadScene("Level1");

    }

void StartNewGame()
{
    int slotToSave;

    if (saveDataList.Count < maxSaves)
    {
        slotToSave = saveDataList.Count + 1;
    }
    else
    {
        slotToSave = saveDataList[0].slotNumber; // Overwrite oldest
    }

    Vector3 playerPosition = new Vector3(510.473f, 5.171842f, 214.0215f);

    SaveData newData = new SaveData
    {
        timestamp = System.DateTime.Now.Ticks,
        levelIndex = 5,
        playerPosition = playerPosition
    };

    string json = JsonUtility.ToJson(newData);
    File.WriteAllText(Application.persistentDataPath + "/Save" + slotToSave + ".json", json);

    PlayerPrefs.SetInt("CurrentSaveSlot", slotToSave); // to use later in game
    PlayerPrefs.SetFloat("PlayerX", playerPosition.x);
    PlayerPrefs.SetFloat("PlayerY", playerPosition.y);
    PlayerPrefs.SetFloat("PlayerZ", playerPosition.z);
    SceneManager.LoadScene(5);
}

}
