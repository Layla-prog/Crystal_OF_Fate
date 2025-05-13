using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FinalBossAI : MonoBehaviour
{
    public Animator animator;
    public float maxHealth = 200f;
    private float currentHealth;

    public float detectRange = 15f;
    public float attackRange = 3f;
    public float attackCooldown = 2f;
    public Transform player;

    private float attackTimer;
    private bool isDead = false;
    private NavMeshAgent agent;

    // movement dodge offset
    public float dodgeDistance = 3f;
    public float dodgeSpeed = 5f;
    private bool isDodging = false;
    private Vector3 dodgeTarget;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        agent = GetComponent<NavMeshAgent>();

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead || player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        FacePlayer();

        if (distance <= detectRange && distance > attackRange)
        {
            MoveTowardsPlayer();
        }
        else
        {
            agent.SetDestination(transform.position);
            animator.SetFloat("Speed", 0f);
        }

        if (distance <= attackRange && attackTimer <= 0f)
        {
            ChooseAttack();
            attackTimer = attackCooldown;
        }

        attackTimer -= Time.deltaTime;
    }
    void MoveTowardsPlayer()
    {
        if (!agent.enabled) return;

        agent.SetDestination(player.position);
        animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    void FacePlayer()
    {
        Vector3 lookDir = player.position - transform.position;
        lookDir.y = 0;
        if (lookDir != Vector3.zero)
        {
            transform.forward = Vector3.Lerp(transform.forward, lookDir.normalized, Time.deltaTime * 10f);
        }
    }

    void ChooseAttack()
    {
        animator.SetFloat("Speed", 0f);
        int attackIndex = Random.Range(0, 2); 
        animator.SetInteger("AttackIndex", attackIndex);
        animator.SetTrigger("Attack");
    }


    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        animator.SetTrigger("Hit");

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        animator.SetTrigger("Die");
        agent.isStopped = true;
        this.enabled = false;
    }

    public void DealDamage()
    {
        if (player != null)
        {
            var target = player.GetComponent<PlayerHealth>();
            if (target != null)
            {
                target.TakeDamage(25f);
            }
        }
    }
}
