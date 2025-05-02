using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPC : MonoBehaviour
{
    public string npcName;
    public Dialogue dialogueData;
    public NPCType npcType;

    private int currentLineIndex = 0;
    private bool playerInRange = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (dialogueData != null && currentLineIndex < dialogueData.lines.Length)
            {
                var line = dialogueData.lines[currentLineIndex];
                DialogueSystem.instance.ShowDialogue(line.speakerName, line.lineText);
                currentLineIndex++;
            }
            else
            {
                DialogueSystem.instance.HideDialogue();
                currentLineIndex = 0;

                TriggerNPCEvent();
            }
        }*/

        if (!playerInRange) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (dialogueData != null && currentLineIndex < dialogueData.lines.Length)
            {
                var line = dialogueData.lines[currentLineIndex];
                DialogueSystem.instance.ShowDialogue(line.speakerName, line.lineText);
                currentLineIndex++;
            }
            else
            {
                SkipDialogue();
            }
        }

        if (Input.GetKeyDown(KeyCode.Return)) // ENTER key
        {
            SkipDialogue();
        }
    }

    void TriggerNPCEvent()
    {
        NPCManager manager = FindObjectOfType<NPCManager>();

        switch (npcType)
        {
            case NPCType.Craftsman:
                manager.CraftingSystem(this.gameObject);
                break;
            case NPCType.Archer:
                manager.RecruitArcher(this.gameObject);
                break;
            case NPCType.PotionCrafter:
                manager.HandlePotionCrafting();
                break;
            case NPCType.Healer:
                manager.HandleHealing();
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            DialogueSystem.instance.HideDialogue();
        }
    }

    public void SkipDialogue()
    {
        DialogueSystem.instance.HideDialogue();
        currentLineIndex = 0;
        TriggerNPCEvent();
    }


}
