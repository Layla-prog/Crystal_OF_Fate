using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public float attackRange = 2f;
    public int attackDamage = 10;
    public Transform attackPoint;
    public LayerMask enemyLayer;
    public Animator animator;

    private bool isBlocking = false;

    private PlayerHealth playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Right click
        {
            Attack("1H_Melee_Attack_Slice_Diagonal");
        }
        else if (Input.GetKeyDown(KeyCode.R)) // Stab attack
        {
            Attack("2H_Melee_Attack_Stab");
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isBlocking = true;
            animator.SetBool("IsBlocking", true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isBlocking = false;
            animator.SetBool("IsBlocking", false);
        }
    }

    void Attack(string animationName)
    {
        animator.Play(animationName);

        // Detect enemies in range
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            EnemyHealth health = enemy.GetComponent<EnemyHealth>();
            if (health != null)
            {
                health.TakeDamage(attackDamage);
            }
        }
    }

    //  player takes damage
    public void TakeHit(bool blocked)
    {
        if (blocked)
        {
            animator.SetTrigger("Block_Hit");
        }
        else
        {
            animator.SetTrigger("Hit-B");
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(10); 
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public bool IsBlocking()
    {
        return isBlocking;
    }
}
