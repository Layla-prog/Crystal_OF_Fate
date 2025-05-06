using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CharacterController))]
public class KateFollowAndCombat : MonoBehaviour
{
    //Follower Setting
    public Transform player;
    public float followSpeed = 3.0f;
    public float stopDistance = 2f;

    //Combat Settings
    public float attackRange = 10f;
    public float meleeRange = 2f;
    public float attackCoolDown = 2f;
    public GameObject arrowPrefab;
    public Transform firePoint;
    public LayerMask enemyLayer;

    private float lastAttackTime;
    private bool isActive = false;


    private Animator animator;
    private CharacterController characterController;


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
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        // Find firePoint in hierarchy
        if (firePoint == null)
        {
            firePoint = transform.Find("upperarm.r/lowerarm.r/wrist.r/hand.r/handslot.r/2H_Crossbow/FirePoint"); 
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
        //float moveSpeed = (distance > stopDistance) ? followSpeed : 0f;

        //animator.SetFloat("Speed", moveSpeed);

        HandleFollow(distance);

        GameObject enemy = FindNearestEnemy();

        if (enemy != null && Vector3.Distance(transform.position, enemy.transform.position) <= attackRange)
        {
            Attack(enemy);
        }
    }

    void HandleFollow(float distance)
    {
        /*if (player == null || animator == null || characterController == null) return;

        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        float distance = direction.magnitude;

        if (distance > stopDistance)
        {
            Vector3 moveDir = direction.normalized * followSpeed;
            Vector3 movement = moveDir * Time.deltaTime;

            characterController.Move(movement);

            if (moveDir != Vector3.zero)
            {
                Quaternion targetRot = Quaternion.LookRotation(moveDir, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 10f * Time.deltaTime);
            }

            animator.SetFloat("Speed", moveDir.magnitude); 
        }
        else
        {
            animator.SetFloat("Speed", 0f);
        }*/
        if (player == null || animator == null || characterController == null) return;

        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        float distToPlayer = direction.magnitude;

        if (distToPlayer > stopDistance)
        {
            Vector3 moveDir = direction.normalized;
            Vector3 movement = moveDir * followSpeed * Time.deltaTime;

            // Prevent stacking by checking ahead
            if (!Physics.CapsuleCast(transform.position + Vector3.up * 0.5f, transform.position + Vector3.up * 1.5f, 0.3f, moveDir, out RaycastHit hit, movement.magnitude, enemyLayer))
            {
                characterController.Move(movement);
                Quaternion targetRot = Quaternion.LookRotation(moveDir, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 10f * Time.deltaTime);
            }

            animator.SetFloat("Speed", moveDir.magnitude);
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
            TriggerRanged();
        }

        /*animator.SetTrigger("isShooting");

        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);
        arrow.GetComponent<Rigidbody>().velocity = firePoint.forward * 25f;*/
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
                enemy.TakeDamage(20); 
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
        arrow.GetComponent<Rigidbody>().velocity = firePoint.forward * 25f;

        yield return new WaitForSeconds(0.3f); // Short delay for reload
        animator.SetBool("isShooting", false);
        animator.SetBool("isReloading", true);

        yield return new WaitForSeconds(0.5f); // Reload duration
        animator.SetBool("isReloading", false);
        animator.SetBool("isAiming", false);
    }

    IEnumerator ResetBool(string param, float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.SetBool(param, false);
    }

}
