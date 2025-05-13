using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MirokuCraftPotion : MonoBehaviour
{
    public Animator animator;
    public GameObject strengthPotionPrefab;
    public GameObject staminaPotionPrefab;


    public Transform handTransform;
    public Transform giveTarget; // player’s potion anchor
    public float giveDuration = 1.5f;

    public float followDistance = 3f;
    public Vector3 followOffset = new Vector3(-2f, 0, -2f);

    private bool isCrafting = false;
    private string currentPotionType;
    private GameObject player;

    private NavMeshAgent agent;
    private bool shouldFollowPlayer = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (shouldFollowPlayer && player != null && agent != null)
        {
            MoveToPlayerAtSafeDistance();

            float speed = agent.velocity.magnitude;
            animator.SetFloat("Speed", speed);
        }
    }

    void MoveToPlayerAtSafeDistance()
    {
        /*float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer > followDistance)
        {
            // Follow the player
            agent.SetDestination(player.transform.position);
        }
        else
        {
            // Stop close to player, at safe distance
            Vector3 directionToPlayer = (transform.position - player.transform.position).normalized;
            Vector3 safeSpot = player.transform.position + directionToPlayer * followDistance;
            agent.SetDestination(safeSpot);
        }*/

        Vector3 playerPos = player.transform.position;
        Vector3 playerForward = player.transform.forward;
        Vector3 playerRight = player.transform.right;

        // Adjust offset in world space (e.g. side by side, slightly behind)
        Vector3 targetPos = playerPos + playerRight * followOffset.x + playerForward * followOffset.z;

        float distanceToTarget = Vector3.Distance(transform.position, targetPos);

        if (distanceToTarget > followDistance)
        {
            agent.SetDestination(targetPos);
        }
        else
        {
            agent.SetDestination(transform.position); // Stop moving
        }
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
        shouldFollowPlayer = true;
    }
}
