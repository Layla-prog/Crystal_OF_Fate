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
    public float attackCoolDown = 2f;
    public GameObject arrowPrefab;
    public Transform firePoint;
    public LayerMask enemyLayer;

    //private NavMeshAgent agent;
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

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive || player == null)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            animator.Play("Walking-B"); 
        }

        float distance = Vector3.Distance(player.position, transform.position);
        float moveSpeed = (distance > stopDistance) ? followSpeed : 0f;

        animator.SetFloat("Speed", moveSpeed);

        HandleFollow();

        GameObject enemy = FindNearestEnemy();

        if (enemy != null && Vector3.Distance(transform.position, enemy.transform.position) <= attackRange)
        {
            Attack(enemy);
        }
    }

    void HandleFollow()
    {
        if (player == null || animator == null || characterController == null) return;

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

            animator.SetFloat("Speed", moveDir.magnitude); // Animate based on actual movement
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

        animator.SetTrigger("IsShooting");

        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);
        arrow.GetComponent<Rigidbody>().velocity = firePoint.forward * 25f;
    }
}
