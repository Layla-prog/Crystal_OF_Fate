using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CharacterController))]
public class KateFollowAndCombat : MonoBehaviour
{
    public string characterName = "Kate";

    //Follower Setting
    public Transform player;
    public float followSpeed = 3.0f;
    public float stopDistance = 3.5f;

    //Combat Settings
    public float attackRange = 10f;
    public float meleeRange = 2f;
    public float meleeDamage = 8f;
    public float attackCoolDown = 2f;
    public GameObject arrowPrefab;
    public Transform firePoint;
    public LayerMask enemyLayer;

    public Vector3 followOffset = new Vector3(1.5f, 0, -1.5f); // move Slightly to the right and back


    private float lastAttackTime;
    private bool isActive = false;
    private float verticalVelocity = 0f;
    private float gravity = -9.81f;


    private Animator animator;
    private CharacterController characterController;
    private GameObject rangedTarget;


    public void ActivateFollowing()
    {
        isActive = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator not found!");
        }
        else
        {
            Debug.Log("Animator assigned: " + animator.runtimeAnimatorController?.name);
        }
        characterController = GetComponent<CharacterController>();

        // Assign player reference 
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }

        // Find firePoint in hierarchy
        if (firePoint == null)
        {
            firePoint = transform.Find("Rig/root/hips/spine/chest/upperarm.r/lowerarm.r/wrist.r/hand.r/handslot.r/2H_Crossbow/FirePoint"); 
            if (firePoint == null)
                Debug.LogError("firePoint not found! Check the bone path and ensure the object exists.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive || player == null)
        {
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);

        HandleFollow(distance);

        GameObject enemy = FindNearestEnemy();

        if (enemy != null)
        {
            EnemyHealth eh = enemy.GetComponent<EnemyHealth>();
            float distToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            if (eh != null && eh.currentHealth > 0 && distToEnemy <= attackRange)
            {
                Attack(enemy); // attack but still follow
            }
        }

    }

    void HandleFollow(float distance)
    {
        if (player == null || animator == null || characterController == null) return;


        Vector3 targetPos = player.position + player.TransformDirection(followOffset);
        Vector3 direction = targetPos - transform.position;

        Vector3 moveDir = direction.normalized;
            Vector3 movement = moveDir * followSpeed;

            if (characterController.isGrounded)
            {
                verticalVelocity = -0.5f; // Keeps grounded
            }
            else
            {
                verticalVelocity += gravity * Time.deltaTime;
            }

            movement.y = verticalVelocity; // Apply gravity

        // Avoid collisions with enemies when moving
        if (distance > stopDistance + 0.5f)
        {
            characterController.Move(movement * Time.deltaTime);

            if (moveDir != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDir), 10f * Time.deltaTime);
            }
            
            animator.SetFloat("Speed", moveDir.magnitude);
        }
        else if (distance < stopDistance - 0.5f) // Too close: back away
        {
            Vector3 backOffDir = -moveDir * followSpeed;
            backOffDir.y = verticalVelocity;

            characterController.Move(backOffDir * Time.deltaTime);

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-moveDir), 10f * Time.deltaTime);

            animator.SetFloat("Speed", followSpeed);
        }
        else
        {
            animator.SetFloat("Speed", 0f);
        }

    }


    GameObject FindNearestEnemy()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);

        float shortestDistance = Mathf.Infinity;

        GameObject nearestEnemy = null;

        foreach (Collider hit in hits)
        {
            float dist = Vector3.Distance(transform.position, hit.transform.position);

            if (dist < shortestDistance)
            {
                shortestDistance = dist;
                nearestEnemy = hit.gameObject;
            }
        }

        return nearestEnemy;
    }

    void Attack(GameObject enemy)
    {
        if (Time.time - lastAttackTime < attackCoolDown)
        {
            return;
        }

        lastAttackTime = Time.time;

        transform.LookAt(enemy.transform);

        float dist = Vector3.Distance(transform.position, enemy.transform.position);

        // If within melee range stab
        if (dist < meleeRange)
        {
            TriggerMelee();
        }
        else // Ranged Attack
        {
            rangedTarget = enemy;
            TriggerRanged();
        }
    }

    void TriggerMelee()
    {
        animator.SetBool("isAiming", false);
        animator.SetBool("isReloading", false);
        animator.SetBool("isShooting", false);

        if (!animator.GetBool("isStabbing"))
        {
            animator.SetBool("isStabbing", true);
            StartCoroutine(DealMeleeDamageAfterDelay(0.4f)); 
            StartCoroutine(ResetBool("isStabbing", 0.9f)); 
        }
    }

    IEnumerator DealMeleeDamageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        Collider[] hits = Physics.OverlapSphere(transform.position + transform.forward * 1f, meleeRange, enemyLayer);
        foreach (Collider hit in hits)
        {
            EnemyHealth enemy = hit.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(meleeDamage); 
            }
        }
    }

    void TriggerRanged()
    {
        animator.SetBool("isStabbing", false);

        if (!animator.GetBool("isAiming"))
        {
            animator.SetBool("isAiming", true);
            StartCoroutine(ShootArrowAfterDelay(0.5f)); // Aiming delay
        }
    }

    IEnumerator ShootArrowAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (firePoint == null)
        {
            Debug.LogError("FirePoint not assigned.");
            yield break;
        }

        animator.SetBool("isShooting", true);

        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);

        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        if (rb != null && rangedTarget != null)
        {
            Vector3 targetPoint = rangedTarget.transform.position + Vector3.up * 1.2f; // Aim for chest
            Vector3 shootDirection = (targetPoint - firePoint.position).normalized;

            rb.useGravity = false; 
            rb.velocity = shootDirection * 25f;
            rb.drag = 0f;

            arrow.transform.rotation = Quaternion.LookRotation(shootDirection);
        }

        // Prevent hitting Kate immediately
        Collider arrowCol = arrow.GetComponent<Collider>();
        Collider kateCol = GetComponent<Collider>();
        if (arrowCol != null && kateCol != null)
        {
            Physics.IgnoreCollision(arrowCol, kateCol);
        }

        arrow.layer = LayerMask.NameToLayer("Projectile");


        yield return new WaitForSeconds(0.2f); // Short delay for reload
        animator.SetBool("isShooting", false);
        animator.SetBool("isReloading", true);

        yield return new WaitForSeconds(0.3f); // Reload duration
        animator.SetBool("isReloading", false);
        animator.SetBool("isAiming", false);
    }

    IEnumerator ResetBool(string param, float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.SetBool(param, false);
    }

}
