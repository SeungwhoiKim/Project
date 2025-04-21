using UnityEngine;
using UnityEngine.AI;

public class GuardPatrol : MonoBehaviour
{
    [Header("Patrol Points")]
    public Transform[] waypoints;
    public float patrolSpeed = 2f;
    public float pointTolerance = 0.2f;
    public bool loop = true;

    private NavMeshAgent agent;
    private int currentIndex = 0;
    private int direction = 1;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null) agent = gameObject.AddComponent<NavMeshAgent>();

        agent.speed = patrolSpeed;
        agent.autoBraking = false;

        if (waypoints.Length > 0)
            agent.SetDestination(waypoints[0].position);
    }

    void Update()
    {
        if (waypoints.Length == 0) return;

        if (!agent.pathPending && agent.remainingDistance <= pointTolerance)
        {
            AdvanceToNextPoint();
        }
    }

    void AdvanceToNextPoint()
    {
        if (loop)
        {
            currentIndex = (currentIndex + 1) % waypoints.Length;
        }
        else
        {
            if (currentIndex + direction >= waypoints.Length || currentIndex + direction < 0)
                direction = -direction;
            currentIndex += direction;
        }

        agent.SetDestination(waypoints[currentIndex].position);
    }

    public void SetPatrolSpeed(float speed)
    {
        patrolSpeed = speed;
        if (agent != null) agent.speed = speed;
    }
}
