using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    public GameObject healthBarRoot;
    public Image fillImage;
    public float hideDelay = 2f;

    private Coroutine hideCoroutine;

    public void Show(float percent)
    {
        if (healthBarRoot == null || fillImage == null) return;

        healthBarRoot.SetActive(true);
        fillImage.fillAmount = percent;

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

    // Start is called before the first frame update
    void Start()
    {
        if (healthBarRoot != null)
        {
            healthBarRoot.SetActive(false); // Start hidden
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
