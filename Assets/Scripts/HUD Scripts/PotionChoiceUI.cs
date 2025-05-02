using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotionChoiceUI : MonoBehaviour
{
    public GameObject choicePanel;
    public Button strengthButton;
    public Button staminaButton;

    private CharacterControl playerScript;

    // Start is called before the first frame update
    void Start()
    {
        // Find player script
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerScript = player.GetComponent<CharacterControl>();

        // Hide panel initially
        choicePanel.SetActive(false);

        // Hook up button events
        strengthButton.onClick.AddListener(ChooseStrength);
        staminaButton.onClick.AddListener(ChooseStamina);
    }

    // Update is called once per frame
    void Update()
    {
        // Show panel when player presses Q
        if (Input.GetKeyDown(KeyCode.Q))
        {
            choicePanel.SetActive(true);
        }
    }

    public void ChooseStrength()
    {
        if (playerScript != null)
        {
            playerScript.SetCurrentPotionType("Strength");
            ClosePanel();
        }
    }

    public void ChooseStamina()
    {
        if (playerScript != null)
        {
            playerScript.SetCurrentPotionType("Stamina");
            ClosePanel();
        }
    }

    private void ClosePanel()
    {
        choicePanel.SetActive(false);
    }
}
