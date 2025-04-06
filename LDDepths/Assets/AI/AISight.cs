using UnityEngine;
using UnityEngine.AI;

public class AISight : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float sightRange = 10f;
    [SerializeField] private float spawnRange = 10f;
    [SerializeField] private float fieldOfViewAngle = 90f;
    [SerializeField] private float detectionCooldown = 1f;
    [SerializeField] private float sightOffset = 1f;

    private NavMeshAgent agent;
    private float timeSinceLastDetection = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        timeSinceLastDetection += Time.deltaTime;

        if (IsTargetInSight())
        {
            agent.SetDestination(target.position);
        }
        else
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
        if (timeSinceLastDetection > detectionCooldown)
        {
            Vector3 randomDirection = Random.insideUnitSphere * sightRange;
            randomDirection += transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, sightRange, NavMesh.AllAreas);
            agent.SetDestination(hit.position);
            timeSinceLastDetection = 0f;
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