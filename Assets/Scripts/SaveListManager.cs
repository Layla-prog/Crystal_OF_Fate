using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SaveListManager : MonoBehaviour
{
    public GameObject saveSlotPrefab;
    public Transform slotsParent;
    public Sprite defaultPreview;

    public GameObject saveButton;
    public GameObject saveButton1;

    private int maxSaves = 4;

    void Start()
    {
        LoadSaveSlots();
    }

    void LoadSaveSlots()
    {
        // Clear existing slots
        foreach (Transform child in slotsParent)
            Destroy(child.gameObject);

        int saveCount = PlayerPrefs.GetInt("SaveCount", 0);
        int displayedSlots = Mathf.Clamp(saveCount, 0, maxSaves);

        // Control visibility of save buttons
        


        for (int i = 1; i <= displayedSlots; i++)
        {
            string levelName = PlayerPrefs.GetString("Save" + i + "_Level", "Unknown");
            Sprite savedPreview = LoadSavedPreview(i) ?? defaultPreview;

            CreateSaveSlot(i, levelName, savedPreview);
        }

        // Add "New Adventure" button
        CreateNewGameSlot(displayedSlots);
    }

    void CreateSaveSlot(int index, string levelName, Sprite preview)
    {
        GameObject slot = Instantiate(saveSlotPrefab, slotsParent);
        slot.transform.SetSiblingIndex(index - 1);

        slot.GetComponentInChildren<TMP_Text>().text = $"Slot {index} | {levelName}";
        slot.GetComponentInChildren<Image>().sprite = preview;
    }

    void CreateNewGameSlot(int saveCount)
    {
        GameObject slot = Instantiate(saveSlotPrefab, slotsParent);
        slot.GetComponentInChildren<TMP_Text>().text = "NEW ADVENTURE";
        slot.GetComponentInChildren<Image>().sprite = defaultPreview;

        HorizontalLayoutGroup layout = slotsParent.GetComponent<HorizontalLayoutGroup>();
        if (layout != null)
        {
            layout.childAlignment = (saveCount == 0) ? TextAnchor.MiddleCenter : TextAnchor.UpperLeft;
        }
    }

    Sprite LoadSavedPreview(int slotNumber)
    {
        // Placeholder - you'll implement the real preview saving later
        return null;
    }
}
