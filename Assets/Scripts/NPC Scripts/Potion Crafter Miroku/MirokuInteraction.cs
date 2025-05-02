using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirokuInteraction : MonoBehaviour
{
    public GameObject potionPrefab;
    public Transform handAnchor;
    public Transform giveTarget; // Where the potion ends up (e.g. player's hand)
    private GameObject heldPotion;

    private Animator animator;
    private bool isInteracting = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void InteractWithPlayer()
    {
        if (isInteracting) return;

        isInteracting = true;
        animator.SetTrigger("Interact");

        Invoke(nameof(BeginCrafting), 1.2f); // Wait for interaction animation
    }

    void BeginCrafting()
    {
        animator.SetTrigger("Craft");

        // Spawn potion and attach to hand
        heldPotion = Instantiate(potionPrefab, handAnchor.position, handAnchor.rotation, handAnchor);
        Invoke(nameof(GivePotion), 3f); // After crafting duration
    }

    void GivePotion()
    {
        animator.SetTrigger("GivePotion");

        // Detach potion from Eedie
        heldPotion.transform.SetParent(null);

        // Find the player's PotionAnchor
        GameObject player = GameObject.FindWithTag("Player");
        Transform playerHandAnchor = player.transform.Find("hips/handslot.l/PotionAnchor");

        if (playerHandAnchor != null)
        {
            // Attach potion to player's hand
            heldPotion.transform.SetParent(playerHandAnchor);
            heldPotion.transform.localPosition = Vector3.zero;
            heldPotion.transform.localRotation = Quaternion.identity;
        }

        isInteracting = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
