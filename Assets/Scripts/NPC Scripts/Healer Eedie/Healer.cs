using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Healer : MonoBehaviour
{
    public Animator animator;
    public float healAmount = 25f;
    public float healThreshold = 70f;
    public float healingDistance = 2f;
    public float followDistance = 5f;

    public ParticleSystem healingParticles;

    private PlayerHealth playerHealth;
    private NavMeshAgent agent;
    private bool isHealing = false;


    void Start()
    {
        // Get PlayerHealth
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player not found! Make sure the player has the 'Player' tag.");
            return;
        }

        playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealth component missing from player!");
            return;
        }

        // Subscribe to OnDamaged event
        playerHealth.OnDamaged += HandlePlayerDamaged;

        // Get components
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (healingParticles != null)
            healingParticles.Stop();

    }

    void Update()
    {
        // Update movement animation
        if (agent != null && animator != null)
        {
            float speed = agent.velocity.magnitude;
            animator.SetFloat("Speed", speed);
        }

        if (!isHealing)
        {
            MoveToPlayerAtSafeDistance();
        }
    }

    void HandlePlayerDamaged(float currentHealth)
    {
        if (currentHealth < healThreshold && !isHealing)
        {
            Debug.Log("Player health below threshold, healer moving.");
            StopAllCoroutines();
            MoveToPlayerAndHeal();
        }
    }

    void MoveToPlayerAtSafeDistance()
    {
        // Check if the healer is too far from the player
        float distanceToPlayer = Vector3.Distance(transform.position, playerHealth.transform.position);

        if (distanceToPlayer > followDistance)
        {
            // Move towards the player but keep a safe distance
            agent.SetDestination(playerHealth.transform.position);
        }
        else
        {
            // Keep at the safe distance and stop moving
            Vector3 directionToPlayer = (playerHealth.transform.position - transform.position).normalized;
            Vector3 safePosition = playerHealth.transform.position - directionToPlayer * followDistance;
            if (Vector3.Distance(agent.destination, safePosition) > 0.5f)
            {
                agent.SetDestination(safePosition);
            }
        }
    }

    void MoveToPlayerAndHeal()
    {
        Vector3 targetPos = playerHealth.transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPos, out hit, 2f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
            StartCoroutine(MoveAndHeal());
        }
        else
        {
            Debug.LogWarning("Cannot find NavMesh near player.");
            isHealing = false;
        }
    }


    IEnumerator MoveAndHeal()
    {
        // Set destination toward player
        agent.SetDestination(playerHealth.transform.position);
        Debug.Log("Destination set to player: " + playerHealth.transform.position);

        // Wait until within healing distance
        while (Vector3.Distance(transform.position, playerHealth.transform.position) > healingDistance)
        {
            agent.SetDestination(playerHealth.transform.position); // keep updating
            yield return null;
        }

        if (playerHealth == null || playerHealth.isDead)
        {
            Debug.Log("Player is dead. Cancelling healing.");
            yield break;
        }

        isHealing = true;

        // Stop agent
        agent.ResetPath();
        animator.SetFloat("Speed", 0f);

        //while (playerHealth.currentHealth < 100f)
        //{
            // Play heal animation and particles
            animator.SetBool("IsHealing", true);
            if (healingParticles != null) healingParticles.Play();

            yield return new WaitForSeconds(2f); // time to "cast" healing

            // Apply healing, making sure not to exceed the max health (100)
            float missingHealth = 100f - playerHealth.currentHealth;
            float healAmountToApply = Mathf.Min(healAmount, missingHealth);

            playerHealth.RestoreHealth(healAmountToApply);
            Debug.Log("Healer healed player by " + healAmount);

        // Stop healing
            animator.SetBool("IsHealing", false);
            if (healingParticles != null) healingParticles.Stop();   

            //yield return new WaitForSeconds(3f);
        //}

        isHealing = false;

    }
}
