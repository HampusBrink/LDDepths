using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class AISight : MonoBehaviour
{
    [Header("Spawn Points")] [SerializeField]
    private Transform[] patrolPoints;

    [SerializeField] private Transform target;
    [SerializeField] private float sightRange = 10f;
    [SerializeField] private float loseSightRange = 20f;
    [SerializeField] private float spawnRange = 10f;
    [SerializeField] private float fieldOfViewAngle = 90f;
    [SerializeField] private float detectionCooldown = 10f;
    [SerializeField] private float sightOffset = 1f;

    private NavMeshAgent agent;
    private float timeSinceLastDetection = 0f;
    private int _counter;
    private bool hasAggro;
    private Transform _previousPatrolPoint;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        timeSinceLastDetection += Time.deltaTime;

        if (IsTargetInSight())
        {
            if (!agent.hasPath)
            {
                agent.SetDestination(target.position);
                hasAggro = true;
            }
        }

        if (hasAggro)
        {
            if (Vector3.Distance(transform.position, target.position ) > loseSightRange)
            {
                hasAggro = false;
            }
        }
        else if(!hasAggro)
        {
            Patrol();
        }
    }

    bool IsTargetInSight()
    {
        Vector3 sightOrigin = transform.position - transform.forward * sightOffset;

        // Check if the target is within the sight range
        float distanceToTarget = Vector3.Distance(sightOrigin, target.position);
        if (distanceToTarget <= sightRange)
        {
            // Check if the target is within the AI's field of view
            Vector3 directionToTarget = (target.position - sightOrigin).normalized;
            float angleBetweenAIandTarget = Vector3.Angle(transform.forward, directionToTarget);

            if (angleBetweenAIandTarget <= fieldOfViewAngle / 2)
            {
                // Perform a raycast to check if there are obstacles in the way
                RaycastHit hit;
                if (Physics.Raycast(sightOrigin, directionToTarget, out hit, sightRange))
                {
                    if (hit.transform == target)
                    {
                        // Target is visible
                        return true;
                    }
                }
            }
        }

        return false;
    }

    void Patrol()
    {
        if (Vector3.Distance(transform.position, target.position) > spawnRange)
        {
            Vector3 randomDirection = Random.onUnitSphere * spawnRange + transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, spawnRange, NavMesh.AllAreas);
            agent.transform.position = hit.position;
        }

        if (timeSinceLastDetection > detectionCooldown)
        {
            var pointClosestToPlayer = patrolPoints.FirstOrDefault();
            foreach (var patrolPoint in patrolPoints)
            {
                if (Vector3.Distance(patrolPoint.position, target.transform.position) <
                    Vector3.Distance(pointClosestToPlayer.position, target.transform.position))
                {
                    pointClosestToPlayer = patrolPoint;
                }
            }

            if (_previousPatrolPoint == pointClosestToPlayer)
            {
                pointClosestToPlayer = patrolPoints[Random.Range(0, patrolPoints.Length)];
            }
            _previousPatrolPoint = pointClosestToPlayer;
            agent.SetDestination(pointClosestToPlayer.position);
            _counter++;
            if (_counter == patrolPoints.Length) _counter = 0;
            timeSinceLastDetection = 0;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Vector3 sightOrigin = transform.position - transform.forward * sightOffset;

        Gizmos.DrawWireSphere(sightOrigin, sightRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, spawnRange);

        Gizmos.color = Color.red;
        Vector3 leftBoundary = Quaternion.Euler(0, -fieldOfViewAngle / 2, 0) * transform.forward * sightRange;
        Vector3 rightBoundary = Quaternion.Euler(0, fieldOfViewAngle / 2, 0) * transform.forward * sightRange;

        Gizmos.DrawLine(sightOrigin, sightOrigin + leftBoundary);
        Gizmos.DrawLine(sightOrigin, sightOrigin + rightBoundary);
    }
}