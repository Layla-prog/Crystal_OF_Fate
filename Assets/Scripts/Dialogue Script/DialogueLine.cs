using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueLine 
{
    public string speakerName;
    [TextArea(3, 6)]
    public string lineText;
    
}
