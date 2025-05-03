using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinAI : MonoBehaviour
{
    public float detectRange = 10f;
    public float attackRange = 2f;
    public float moveSpeed = 2f;
    public float attackCooldown = 1.5f;
    public int attackDamage = 10;
    public Animator animator;

    private Transform player;
    private float attackTimer = 0f;
    private CharacterController controller;
    private bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead || player == null) return;

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

        attackTimer -= Time.deltaTime;
    }

    void FacePlayer()
    {
        Vector3 lookDir = player.position - transform.position;
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
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);
        if (dist <= attackRange + 0.2f)
        {
            PlayerCombat combat = player.GetComponent<PlayerCombat>();
            if (combat != null)
            {
                combat.TakeHit(combat.IsBlocking()); // Add IsBlocking() method if needed
            }
        }
    }
}
