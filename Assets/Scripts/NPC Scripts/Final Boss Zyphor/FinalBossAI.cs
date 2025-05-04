using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private CharacterController controller;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        controller = GetComponent<CharacterController>();

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
            animator.SetFloat("Speed", 1f);
            MoveTowardsPlayer();
        }
        else
        {
            animator.SetFloat("Speed", 0f);
        }

        if (distance <= attackRange && attackTimer <= 0f)
        {
            ChooseAttack();
            attackTimer = attackCooldown;
        }

        if (animator.GetBool("Phase2"))
        {
            TryDodgePlayer();
        }

        attackTimer -= Time.deltaTime;
    }

    void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        controller.SimpleMove(direction * 2.5f);
    }

    void FacePlayer()
    {
        Vector3 lookDir = player.position - transform.position;
        lookDir.y = 0;
        if (lookDir != Vector3.zero)
            transform.forward = lookDir.normalized;
    }

    void ChooseAttack()
    {
        int attackIndex = Random.Range(0, 2); // 0 = bear_Strike1, 1 = bear_Strike2
        animator.SetInteger("AttackIndex", attackIndex);
        animator.SetTrigger("Attack");
    }

    void TryDodgePlayer()
    {
        float dodgeChance = 0.3f;
        if (Random.value < dodgeChance)
        {
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            float rand = Random.value;

            Vector3 dodgeDir = Vector3.zero;
            if (rand < 0.25f) dodgeDir = Vector3.left;
            else if (rand < 0.5f) dodgeDir = Vector3.right;
            else if (rand < 0.75f) dodgeDir = Vector3.back;
            else dodgeDir = Vector3.forward;

            Dodge(dodgeDir);
        }
    }

    public void Dodge(Vector3 direction)
    {
        if (isDead) return;

        string trigger = "Dodge_";
        if (direction == Vector3.forward) trigger += "Forward";
        else if (direction == Vector3.back) trigger += "Backward";
        else if (direction == Vector3.left) trigger += "Left";
        else if (direction == Vector3.right) trigger += "Right";

        animator.SetTrigger(trigger);
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        animator.SetTrigger("Hit");

        if (currentHealth <= maxHealth * 0.5f && !animator.GetBool("Phase2"))
        {
            animator.SetBool("Phase2", true);
            // Optional: Play taunt or cheer
            animator.SetTrigger("Cheer");
        }

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        animator.SetTrigger("Die");
        controller.enabled = false;
        this.enabled = false;
    }
}
