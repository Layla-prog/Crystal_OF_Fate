using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealthUI : MonoBehaviour
{
    public GameObject healthBarRoot;
    public Image fillImage;
    public float hideDelay = 3f;

    private Coroutine hideCoroutine;

    public void ShowHealthBar(float healthPercent)
    {
        healthBarRoot.SetActive(true);
        fillImage.fillAmount = healthPercent;

        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }

        hideCoroutine = StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(hideDelay);
        healthBarRoot.SetActive(false);
    }

   
}
