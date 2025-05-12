using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirokuCraftPotion : MonoBehaviour
{
    public Animator animator;
    public GameObject strengthPotionPrefab;
    public GameObject staminaPotionPrefab;


    public Transform handTransform;
    public Transform giveTarget; // player’s potion anchor
    public float giveDuration = 1.5f;

    private bool isCrafting = false;
    private string currentPotionType;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void TryCraftPotion(string type)
    {
        if (isCrafting) return;

        currentPotionType = type;
        StartCoroutine(CraftAndGivePotion());
    }

    IEnumerator CraftAndGivePotion()
    {
        isCrafting = true;

        animator.SetTrigger("Craft");

        yield return new WaitForSeconds(2.0f); 

        // Select correct prefab
        GameObject potionPrefab = null;
        if (currentPotionType == "Strength")
            potionPrefab = strengthPotionPrefab;
        else if (currentPotionType == "Stamina")
            potionPrefab = staminaPotionPrefab;

        if (potionPrefab == null)
        {
            Debug.LogWarning("No valid potion prefab for type: " + currentPotionType);
            isCrafting = false;
            yield break;
        }

        // Spawn potion in hand
        GameObject potion = Instantiate(potionPrefab, handTransform.position, Quaternion.identity, handTransform);
        yield return new WaitForSeconds(1.0f); 

        // Play give animation
        animator.SetTrigger("GivePotion");

        // Detach potion and move it to the player's potion anchor
        potion.transform.SetParent(null);
        Vector3 startPos = potion.transform.position;
        Vector3 targetPos = giveTarget.position;

        float elapsed = 0f;
        while (elapsed < giveDuration)
        {
            potion.transform.position = Vector3.Lerp(startPos, targetPos, elapsed / giveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        potion.transform.SetParent(giveTarget, false);
        potion.transform.localPosition = Vector3.zero;
        potion.transform.localRotation = Quaternion.identity;

        // Notify player
        player.SendMessage("SetCurrentPotionType", currentPotionType, SendMessageOptions.DontRequireReceiver);

        isCrafting = false;
    }
}
