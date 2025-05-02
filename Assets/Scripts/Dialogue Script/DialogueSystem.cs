using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TMP_Text nameText;
    public TMP_Text dialogueText;

    public static DialogueSystem instance;

    void Awake()
    {
        instance = this;
        dialoguePanel.SetActive(false);
    }

    public void ShowDialogue(string speaker, string dialogue)
    {
        nameText.text = speaker;
        dialogueText.text = dialogue;
        dialoguePanel.SetActive(true);
    }

    public void HideDialogue()
    {
        dialoguePanel.SetActive(false);
    }
    
}
