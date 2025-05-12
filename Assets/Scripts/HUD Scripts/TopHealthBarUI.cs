using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TopHealthBarUI : MonoBehaviour
{
    public TMP_Text nameText;
    public Image fillImage;

    private float hideDelay = 5f;
    private float timer;
    private bool isVisible = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isVisible)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                gameObject.SetActive(false);
                isVisible = false;
            }
        }
    }

    public void UpdateHealth(string characterName, float percent)
    {
        nameText.text = characterName;
        fillImage.fillAmount = percent;

        gameObject.SetActive(true);
        timer = hideDelay;
        isVisible = true;
    }
}
