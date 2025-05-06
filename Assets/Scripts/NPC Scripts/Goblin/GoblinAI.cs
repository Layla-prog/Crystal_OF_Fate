using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GoblinAI : MonoBehaviour
{
    public float detectRange = 10f;
    public float attackRange = 2f;
    public float moveSpeed = 2f;
    public float attackCooldown = 1.5f;
    public int attackDamage = 10;
    public Animator animator;

    //private Transform player;
    private Transform currentTarget;
    private float attackTimer = 0f;
    private CharacterController controller;
    private bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();

        //player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (isDead || player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;

        float stopBuffer = 0.3f;

        if (distance <= detectRange)
        {
            FacePlayer();

            if (distance > attackRange + stopBuffer)
            {
                Vector3 move = direction * moveSpeed;
                controller.SimpleMove(move); // uses gravity, smoother than .Move
                animator.SetFloat("Speed", 1f); // Triggers walk
            }
            else
            {
                animator.SetFloat("Speed", 0f); // Stop walking

                if (attackTimer <= 0f)
                {
                    animator.SetTrigger("Attack");
                    attackTimer = attackCooldown;
                }
            }
        }
        else
        {
            animator.SetFloat("Speed", 0f);
            controller.SimpleMove(Vector3.zero);
        }

        attackTimer -= Time.deltaTime;*/

        if (isDead) return;

        FindNearestTarget();

        if (currentTarget == null)
        {
            return;
        }

        float distance = Vector3.Distance(transform.position, currentTarget.position);
        Vector3 direction = (currentTarget.position - transform.position).normalized;
        direction.y = 0f;

        //float stopBuffer = 0.3f;

        if (distance <= detectRange)
        {
            FindNearestTarget();

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
                }
            }
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
        /*Vector3 lookDir = player.position - transform.position;
        lookDir.y = 0;
        if (lookDir != Vector3.zero)
            transform.forward = lookDir.normalized;*/

        if (currentTarget == null) return;

        Vector3 lookDir = currentTarget.position - transform.position;
        lookDir.y = 0;
        if (lookDir != Vector3.zero)
            transform.forward = lookDir.normalized;
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;
        animator.SetBool("IsDead", true);
        this.enabled = false;
        controller.enabled = false;
    }

    public void DealDamage()
    {
        /* if (player == null) return;

         float dist = Vector3.Distance(transform.position, player.position);
         if (dist <= attackRange + 0.2f)
         {
             PlayerCombat combat = player.GetComponent<PlayerCombat>();
             if (combat != null)
             {
                 combat.TakeHit(combat.IsBlocking()); // Add IsBlocking() method if needed
             }
         }*/

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
                    playerCombat.TakeHit(playerCombat.IsBlocking());
                }
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(attackDamage);
                }
            }
            else if (currentTarget.CompareTag("Ally"))
            {
                var npcCombat = currentTarget.GetComponent<NPCCombat>();
                if (npcCombat != null)
                {
                    npcCombat.TakeDamage(attackDamage);
                }
            }
        }
    }
}
