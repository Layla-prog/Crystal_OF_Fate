using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float patrolWaitTime = 2f;
    public float detectionRange = 10f;

    private int currentPatrolIndex;
    private float waitTimer;
    private Transform player;
    private NavMeshAgent agent;
    private bool playerDetected = false;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (patrolPoints.Length > 0)
        {
            agent.SetDestination(patrolPoints[0].position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            playerDetected = true;
            agent.isStopped = true; // Stop moving to patrol points
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z)); // Look at player
        }
        else
        {
            playerDetected = false;
            agent.isStopped = false;
            Patrol();
        }
    }

    void Patrol()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            waitTimer += Time.deltaTime;

            if (waitTimer >= patrolWaitTime)
            {
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
                agent.SetDestination(patrolPoints[currentPatrolIndex].position);
                waitTimer = 0f;
            }
        }
    }

    public bool PlayerIsDetected()
    {
        return playerDetected;
    }

    public Transform GetPlayer()
    {
        return player;
    }
}
