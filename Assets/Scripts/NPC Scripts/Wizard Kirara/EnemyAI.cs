using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float patrolWaitTime = 2f;
    public float detectionRadius = 19f;

    private int currentPatrolIndex;
    private float waitTimer;
    private NavMeshAgent agent;

    private Transform currentTarget;
    private List<Transform> detectedTargets = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //currentTarget = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (patrolPoints.Length > 0)
        {
            agent.SetDestination(patrolPoints[0].position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        DetectTargets();

        if (PlayerIsDetected())
        {
            agent.isStopped = true;
            FaceTarget();
        }
        else
        {
            agent.isStopped = false;
            Patrol();
        }
    }

    void DetectTargets()
    {
        detectedTargets.Clear();
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                detectedTargets.Add(hit.transform);
            }
        }

        currentTarget = detectedTargets.Count > 0 ? GetNearestTarget() : null;
    }

    Transform GetNearestTarget()
    {
        Transform closest = null;
        float minDist = Mathf.Infinity;

        foreach (var target in detectedTargets)
        {
            float dist = Vector3.Distance(transform.position, target.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = target;
            }
        }
        return closest;
    }

    void FaceTarget()
    {
        if (currentTarget == null) return;

        Vector3 lookPos = currentTarget.position - transform.position;
        lookPos.y = 0;
        transform.rotation = Quaternion.LookRotation(lookPos);
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
        return currentTarget != null;
    }

    public Transform GetPlayer()
    {
        return currentTarget;
    }
}
