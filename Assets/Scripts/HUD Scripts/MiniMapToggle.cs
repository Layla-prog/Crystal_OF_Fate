using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapToggle : MonoBehaviour
{
    public GameObject miniMapSmall; //top-right
    public GameObject miniMapFull; //full-screen

    private bool isFullMapOpen = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMap();
        }
    }

    public void ToggleMap()
    {
        isFullMapOpen = !isFullMapOpen;
        miniMapSmall.SetActive(!isFullMapOpen);
        miniMapFull.SetActive(isFullMapOpen);
    }
}
