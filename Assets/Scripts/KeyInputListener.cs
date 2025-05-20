using UnityEngine;
using TMPro;

public class KeyInputListener : MonoBehaviour
{
    public TMP_Text keyText; // Assign in the Inspector

    private string tempKey = ""; // Store temporarily pressed key

    void Update()
    {
        if (Input.anyKeyDown)
        {
            foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    // Ignore mouse buttons
                    if (key == KeyCode.Mouse0 || key == KeyCode.Mouse1 || key == KeyCode.Mouse2)
                        continue;

                    string newKey = key.ToString();
                    string savedKey = PlayerPrefs.GetString("SavedKey", "");

                    // Prevent selecting the same key again
                    if (newKey == savedKey)
                    {
                        Debug.Log("This key is already assigned: " + newKey);
                        return;
                    }

                    Debug.Log("Key Pressed: " + newKey);
                    tempKey = newKey;
                    keyText.text = newKey;
                    break;
                }
            }
        }
    }

    // Call this from the Btn_yes OnClick event
    public void SaveKeyText()
    {
        if (!string.IsNullOrEmpty(keyText.text))
        {
            string savedKey = PlayerPrefs.GetString("SavedKey", "");
            if (keyText.text == savedKey)
            {
                Debug.Log("Key already saved, no update needed.");
                return;
            }

            PlayerPrefs.SetString("SavedKey", keyText.text);
            PlayerPrefs.Save();
            Debug.Log("Saved key: " + keyText.text);
        }
    }

    // Call this from Btn_No OnClick
   public void CancelKeyAssignment()
    {
    Debug.Log("Key assignment canceled.");
    tempKey = ""; // Clear temporary key
    PlayerPrefs.DeleteKey("SavedKey"); // Remove saved key from storage
    keyText.text = "NOT ASSIGNED"; // Update UI
    }

}
