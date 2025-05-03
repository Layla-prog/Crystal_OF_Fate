using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardEnemyAI : MonoBehaviour
{
    //public float detectRange = 10f;
    public float attackRange = 6f;
    public float attackCooldown = 3f;
    public GameObject poisonPotionPrefab;
    public Transform throwPoint;
    public Animator animator;

    //private Transform player;
    private float cooldownTimer;
    private bool isDead = false;

    private EnemyAI patrolAI;

    // Start is called before the first frame update
    void Start()
    {
        /*GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;*/
        patrolAI = GetComponent<EnemyAI>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (isDead || player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectRange)
        {
            FacePlayer();

            if (distance <= attackRange && cooldownTimer <= 0f)
            {
                Attack();
                cooldownTimer = attackCooldown;
            }
        }

        cooldownTimer -= Time.deltaTime;*/

        if (isDead || patrolAI == null || !patrolAI.PlayerIsDetected()) return;

        Transform player = patrolAI.GetPlayer();
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange && cooldownTimer <= 0f)
        {
            FacePlayer(player);
            Attack();
            cooldownTimer = attackCooldown;
        }

        cooldownTimer -= Time.deltaTime;
    }

    void FacePlayer(Transform player)
    {
        Vector3 dir = (player.position - transform.position).normalized;
        dir.y = 0;
        if (dir != Vector3.zero)
            transform.forward = dir;
    }

    void Attack()
    {
        animator.SetTrigger("Attack"); // triggers spellcast_shoot
        Invoke(nameof(ThrowPoisonPotion), 0.5f); // sync with animation
    }

    void ThrowPoisonPotion()
    {
        Instantiate(poisonPotionPrefab, throwPoint.position, throwPoint.rotation);
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
