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

        player = patrolAI.GetPlayer();
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        /*if (distance <= attackRange && cooldownTimer <= 0f)
        {
            FacePlayer(player);
            Attack();
            cooldownTimer = attackCooldown;
        }*/

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
        /*Vector3 dir = (player.position - transform.position).normalized;
        dir.y = 0;
        if (dir != Vector3.zero)
            transform.forward = dir;*/
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

    /*void ThrowPoisonPotion()
    {
        Transform target = patrolAI.GetPlayer();
        if (target == null) return;

        GameObject potion = Instantiate(poisonPotionPrefab, throwPoint.position, Quaternion.identity);

        //Vector3 targetPoint = target.position + Vector3.up * 1.2f;
        //Vector3 direction = (target.position - throwPoint.position).normalized;
        //direction.y = 0.2f; // small upward arc

        Rigidbody rb = potion.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 targetPoint = target.position + Vector3.up * 1.2f;
            //Vector3 direction = (target.position - throwPoint.position).normalized;

            Vector3 velocity = CalculateBallisticVelocity(targetPoint, throwPoint.position, 2.5f);

            rb.velocity = velocity;

            potion.transform.forward = velocity.normalized;
        }

        if (!potion.TryGetComponent<PoisonProjectile>(out _))
        {
            potion.AddComponent<PoisonProjectile>().damage = 15f;
        }

        //potion.transform.forward = direction;

        PoisonProjectile poisonScript = potion.GetComponent<PoisonProjectile>();
        poisonScript.SetTarget(target);
    }*/

    public void ThrowPotionAtPlayer()
    {
        if (player == null || poisonPotionPrefab == null || throwPoint == null) return;

        Debug.Log("Wizard is throwing a potion!"); // <-- Add this

        GameObject potion = Instantiate(poisonPotionPrefab, throwPoint.position, Quaternion.identity);
        /*PoisonProjectile projectile = potion.GetComponent<PoisonProjectile>();
        if (projectile != null)
        {
            projectile.SetTarget(player);
        }*/

        // Add force toward player
        Rigidbody rb = potion.GetComponent<Rigidbody>();
        if (rb != null)
        {
            /*Vector3 direction = (player.position - throwPoint.position).normalized;
            rb.AddForce(direction * 10f, ForceMode.VelocityChange);*/
            //Vector3 targetPoint = player.position + Vector3.up * 1.2f; // Aim at upper body
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



    /*Vector3 CalculateBallisticVelocity(Vector3 target, Vector3 origin, float arcHeight)
    {
        Vector3 direction = target - origin;
        Vector3 horizontal = new Vector3(direction.x, 0f, direction.z);
        float distance = horizontal.magnitude;
        float yOffset = direction.y;

        float gravity = Mathf.Abs(Physics.gravity.y);
        float initialYSpeed = Mathf.Sqrt(2 * gravity * arcHeight);
        float time = (initialYSpeed + Mathf.Sqrt(initialYSpeed * initialYSpeed + 2 * gravity * yOffset)) / gravity;

        Vector3 velocity = horizontal / time;
        velocity.y = initialYSpeed;
        return velocity;
    }*/


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
