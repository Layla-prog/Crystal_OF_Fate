using UnityEngine;
using TMPro;
using System.Collections;

public class InputSettingsManager : MonoBehaviour
{
    public TMP_Text forwardKeyText;
    public TMP_Text backwardKeyText;
    public TMP_Text leftKeyText;
    public TMP_Text rightKeyText;
    public TMP_Text jumpKeyText;
    public TMP_Text interactKeyText;

    public KeyCode forwardKey;
    public KeyCode backwardKey;
    public KeyCode leftKey;
    public KeyCode rightKey;
    public KeyCode jumpKey;
    public KeyCode interactKey;

    private string keyToRebind = null;

    void Start()
    {
        // Load saved keys or defaults
        forwardKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Forward", "W"));
        backwardKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Backward", "S"));
        leftKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Left", "A"));
        rightKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Right", "D"));
        jumpKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Jump", "Space"));
        interactKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Interact", "E"));

        UpdateUI();
    }

    void UpdateUI()
    {
        forwardKeyText.text = forwardKey.ToString();
        backwardKeyText.text = backwardKey.ToString();
        leftKeyText.text = leftKey.ToString();
        rightKeyText.text = rightKey.ToString();
        jumpKeyText.text = jumpKey.ToString();
        interactKeyText.text = interactKey.ToString();
    }

    public void StartRebinding(string action)
    {
        keyToRebind = action;
        StartCoroutine(WaitForKey());
    }

    private IEnumerator WaitForKey()
    {
        yield return null;

        while (!Input.anyKeyDown)
            yield return null;

        foreach (KeyCode kcode in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(kcode))
            {
                SetKey(keyToRebind, kcode);
                break;
            }
        }
    }

    private void SetKey(string action, KeyCode newKey)
    {
        PlayerPrefs.SetString(action, newKey.ToString());
        PlayerPrefs.Save();

        switch (action)
        {
            case "Forward":
                forwardKey = newKey;
                break;
            case "Backward":
                backwardKey = newKey;
                break;
            case "Left":
                leftKey = newKey;
                break;
            case "Right":
                rightKey = newKey;
                break;
            case "Jump":
                jumpKey = newKey;
                break;
            case "Interact":
                interactKey = newKey;
                break;
        }

        UpdateUI();
    }
}
