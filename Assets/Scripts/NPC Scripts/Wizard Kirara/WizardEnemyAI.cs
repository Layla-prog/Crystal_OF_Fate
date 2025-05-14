using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardEnemyAI : MonoBehaviour
{
    public string characterName = "Kirara";

    //public float detectRange = 10f;
    public float attackRange = 15f;
    public float attackCooldown = 3f;
    public GameObject poisonPotionPrefab;
    public Transform throwPoint;
    public Animator animator;

    private Transform player;
    private float cooldownTimer;
    private bool isDead = false;
    private bool isAttacking = false;

    private EnemyAI patrolAI;

    // Start is called before the first frame update
    void Start()
    {
        patrolAI = GetComponent<EnemyAI>();
    }

    // Update is called once per frame
    void Update()
    {

        if (isDead || patrolAI == null || !patrolAI.PlayerIsDetected()) return;

        player = patrolAI.GetPlayer();
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            FacePlayer();
            if (!isAttacking)
            {
                Attack();
            }
        }

    cooldownTimer -= Time.deltaTime;
    }

    void FacePlayer()
    {
        if (player == null) return;

        Vector3 direction = player.position - transform.position;
        direction.y = 0;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    void ResetAttack()
    {
        isAttacking = false;
    }

    void Attack()
    {
        animator.SetTrigger("AttackRaise"); // triggers spellcast_raise
        Invoke(nameof(TriggerShoot), 1.0f); // Wait until staff is raised
        Invoke(nameof(ResetAttack), attackCooldown);
    }

    void TriggerShoot()
    {
        animator.SetTrigger("AttackShoot"); // Then move to spellcast_shoot
        Invoke(nameof(ThrowPotionAtPlayer), 1.3f); // Throw during shoot animation
    }

    public void ThrowPotionAtPlayer()
    {
        if (player == null || poisonPotionPrefab == null || throwPoint == null) return;

        Debug.Log("Wizard is throwing a potion!"); // <-- Add this

        GameObject potion = Instantiate(poisonPotionPrefab, throwPoint.position, Quaternion.identity);
        

        // Add force toward player
        Rigidbody rb = potion.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 direction = (player.position - throwPoint.position).normalized;
            rb.velocity = direction * 10f;
        }

        Physics.IgnoreCollision(potion.GetComponent<Collider>(), GetComponent<Collider>());

        PoisonProjectile poisonScript = potion.GetComponent<PoisonProjectile>();
        if (poisonScript != null)
        {
            poisonScript.SetTarget(player);
        }
    }


    public void Die()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;
        animator.SetBool("IsDead", true);

        // Disable combat and movement 
        var patrol = GetComponent<EnemyAI>();
        if (patrol != null)
            patrol.enabled = false;

        // Disable CharacterController to prevent further movement
        var controller = GetComponent<CharacterController>();
        if (controller != null)
            controller.enabled = false;

        // Disable all colliders to prevent interactions
        foreach (var col in GetComponentsInChildren<Collider>())
            col.enabled = false;

        // Optionally, destroy the game object after a delay
        Destroy(gameObject, 5f);
    }
}
