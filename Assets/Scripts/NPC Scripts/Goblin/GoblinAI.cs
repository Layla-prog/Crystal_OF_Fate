using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GoblinAI : MonoBehaviour
{
    public string characterName = "Goblin";

    public float detectRange = 10f;
    public float attackRange = 2f;
    public float moveSpeed = 2f;
    public float attackCooldown = 1.5f;
    public int attackDamage = 10;
    public Animator animator;


    public GameObject staminaPotionPrefab; 
    public Transform dropPoint;


    //private Transform player;
    private Transform currentTarget;
    private float attackTimer = 0f;
    private CharacterController controller;
    private bool isDead = false;
    private bool hasDealtDamage = false;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        currentTarget = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead || currentTarget == null) return;

        //FindNearestTarget();

        float distance = Vector3.Distance(transform.position, currentTarget.position);
        Vector3 direction = (currentTarget.position - transform.position).normalized;
        direction.y = 0f;

        //Avoid overlapping
        Vector3 avoidanceForce = AvoidanceForce();
        direction = (direction + avoidanceForce).normalized;

        FaceTarget();

        //float stopBuffer = 0.3f;

        if (distance <= detectRange)
        {

            if (distance > attackRange)
            {
                controller.SimpleMove(direction * moveSpeed);
                animator.SetFloat("Speed", 1f);
            }
            else
            {
                controller.SimpleMove(Vector3.zero);
                animator.SetFloat("Speed", 0f);

                if (attackTimer <= 0f)
                {
                    animator.SetTrigger("Attack");
                    attackTimer = attackCooldown;

                    DealDamage();
                }
            }
        }
        else
        {
            animator.SetFloat("Speed", 0f);
            controller.SimpleMove(Vector3.zero);
        }

        attackTimer -= Time.deltaTime;

    }

    void FindNearestTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Player");

        float shortestDistance = Mathf.Infinity;
        Transform nearest = null;

        foreach (var target in targets)
        {
            float dist = Vector3.Distance(transform.position, target.transform.position);
            if (dist < shortestDistance)
            {
                shortestDistance = dist;
                nearest = target.transform;
            }
        }

        GameObject[] allies = GameObject.FindGameObjectsWithTag("Ally");
        foreach (var ally in allies)
        {
            float dist = Vector3.Distance(transform.position, ally.transform.position);
            if (dist < shortestDistance)
            {
                shortestDistance = dist;
                nearest = ally.transform;
            }
        }

        currentTarget = nearest;
    }

    void FaceTarget()
    {
        if (currentTarget == null) return;

        Vector3 lookDir = currentTarget.position - transform.position;
        lookDir.y = 0;
        if (lookDir != Vector3.zero)
            transform.forward = lookDir.normalized;
    }

    private Vector3 AvoidanceForce()
    {
        Vector3 repulsion = Vector3.zero;

        // Find nearby NPCs (those tagged as "Ally")
        Collider[] nearbyNPCs = Physics.OverlapSphere(transform.position, 2f);
        foreach (var col in nearbyNPCs)
        {
            if (col.gameObject != gameObject && col.CompareTag("Ally"))
            {
                // Calculate the direction away from the NPC
                Vector3 awayFromNPC = transform.position - col.transform.position;
                if (awayFromNPC.magnitude > 0.01f)
                {
                    // Add a repulsion force (can tweak the strength)
                    repulsion += awayFromNPC.normalized / awayFromNPC.magnitude;
                }
            }
        }

        return repulsion.normalized * 1f;
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;
        animator.SetBool("IsDead", true);
        this.enabled = false;
        controller.enabled = false;

        Debug.Log("Goblin died. Dropping potion.");
        // drop potion
        if (staminaPotionPrefab != null)
        {
            Vector3 dropPosition = transform.position + Vector3.up * 1.0f; // 1 unit above ground
            Instantiate(staminaPotionPrefab, dropPosition, Quaternion.identity);
        }
    }

    public void DealDamage()
    {
        if (hasDealtDamage) return;

        hasDealtDamage = true;

        if (currentTarget == null) return;

        float dist = Vector3.Distance(transform.position, currentTarget.position);
        if (dist <= attackRange + 0.2f)
        {
            // Check if it's the player
            if (currentTarget.CompareTag("Player"))
            {
                var playerCombat = currentTarget.GetComponent<PlayerCombat>();
                var playerHealth = currentTarget.GetComponent<PlayerHealth>();
                if (playerCombat != null)
                {
                    playerCombat.TakeHit(false);
                }
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(attackDamage);
                }
            }
        }
    }

    public void ResetDamageFlag()
    {
        hasDealtDamage = false;
    }
}
