using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionZone : MonoBehaviour
{

    public GameObject interactionPrompt; // UI: "Press Q to request potion"
    public GameObject choiceUI;          // UI panel with Strength/Stamina buttons

    private bool playerInRange = false;
    private MirokuCraftPotion craftPotion;

    // Start is called before the first frame update
    void Start()
    {
        interactionPrompt.SetActive(false);
        choiceUI.SetActive(false);
        craftPotion = GetComponentInParent<MirokuCraftPotion>();
    }

    // Update is called once per frame
    void Update()
    {

        if (playerInRange && Input.GetKeyDown(KeyCode.Q))
        {
            interactionPrompt.SetActive(false);
            choiceUI.SetActive(true); // Only show choice panel when prompt was visible
        }
    }

    public void ChooseStrength()
    {
        Debug.Log("Strength button");
        choiceUI.SetActive(false);
        craftPotion.TryCraftPotion("Strength");
    }

    public void ChooseStamina()
    {
        choiceUI.SetActive(false);
        craftPotion.TryCraftPotion("Stamina");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            interactionPrompt.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            interactionPrompt.SetActive(false);
            choiceUI.SetActive(false);
        }
    }       
}
