using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapUI : MonoBehaviour
{
    public GameObject miniMapPanel;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            miniMapPanel.SetActive(!miniMapPanel.activeSelf);
        }
    }
}
