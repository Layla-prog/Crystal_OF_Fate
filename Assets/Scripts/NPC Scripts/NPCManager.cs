using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public GameObject archerFollowerPrefab;
    private GameObject archerFollowerInstance;

    public PlayerHealth playerHealth;
    //public Inventory playerInventory;


    public void CraftingSystem(GameObject npc)
    {
        npc.GetComponent<NPC>().enabled = false;
    }

    public void RecruitArcher(GameObject npc)
    {
        //DialogueSystem.instance.ShowDialogue("Archer Kate", "I'm ready to join your quest!");


        var follow = npc.GetComponent<KateFollowAndCombat>();
        if (follow != null)
        {
            follow.enabled = true;
        }
        npc.SetActive(false); //hide static NPC
        archerFollowerInstance = Instantiate(archerFollowerPrefab, npc.transform.position, Quaternion.identity);

        archerFollowerInstance.GetComponent<KateFollowAndCombat>().ActivateFollowing();

        //npc.GetComponent<NPC>().enabled = false;
    }

    public void HandlePotionCrafting()
    {
        //if (playerInventory.HasResources("Herbs", 5))
        //{
            //DialogueSystem.instance.ShowDialogue("Eedie", "Here's your strength potion!");
            //playerInventory.RemoveResources("Herbs", 5);
            //playerInventory.AddItem("Strength Potion");
        //}
        //else
        //{
          //  DialogueSystem.instance.ShowDialogue("Eedie", "Bring me 5 herbs and I'll brew something for you.");
        //}
    }

    public void HandleHealing()
    {
        if (playerHealth.currentHealth < playerHealth.maxHealth)
        {
            playerHealth.RestoreHealth(50);
            //DialogueSystem.instance.ShowDialogue("Healer Miroku", "You're healed. Be careful out there.");
        }
        //else
        //{
            //DialogueSystem.instance.ShowDialogue("Healer Miroku", "You're already healthy.");
        //}
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
