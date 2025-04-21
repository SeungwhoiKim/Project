using UnityEngine;

public class SquirrelAI : MonoBehaviour
{
    public enum State { Walking, Idle, Flee }
    private State currentState;

    [Header("Speeds")]
    public float moveSpeed = 1f;
    public float rotationSpeed = 5f;
    public float fleeSpeedMultiplier = 1.5f;

    [Header("Durations")]
    public float minWalkDuration = 3f;
    public float maxWalkDuration = 6f;
    public float minIdleDuration = 2f;
    public float maxIdleDuration = 4f;
    public float fleeDuration = 4f;

    [Header("FOV Detection")]
    public float detectionDistance = 5f;
    public float detectionAngle = 60f;
    public float eyeHeight = 1.0f;
    public LayerMask viewMask;

    private Transform player;
    private PlayerController playerCtl;
    private float stateTimer;
    private float currentStateDuration;
    private Vector3 walkDirection;

    private float fleeTimer;
    private float fleeSpeed;

    void Start()
    {
        var go = GameObject.FindGameObjectWithTag("Player");
        if (go != null)
        {
            player = go.transform;
            playerCtl = go.GetComponent<PlayerController>();
        }
        else Debug.LogError("SquirrelAI: Can't find Player tage.");

        SetRandomWalkState();
    }

    void Update()
    {
        if (player == null) return;

        Vector3 toPlayer = player.position - transform.position;
        toPlayer.y = 0;
        float dist = toPlayer.magnitude;
        float halfAngle = detectionAngle * 0.5f;
        float angleFromHead = Vector3.Angle(transform.right, toPlayer);

        if (dist <= detectionDistance && angleFromHead <= halfAngle)
        {
            Vector3 origin = transform.position + Vector3.up * eyeHeight;
            Ray ray = new Ray(origin, toPlayer.normalized);
            if (Physics.Raycast(ray, out RaycastHit hit, detectionDistance, viewMask))
            {
                if (hit.transform == player)
                {
                    if (currentState != State.Flee)
                        EnterFleeState();
                }
            }
        }
        if (currentState == State.Flee)
        {
            fleeTimer -= Time.deltaTime;
            if (fleeTimer <= 0f)
                SetRandomWalkState();
        }

        switch (currentState)
        {
            case State.Walking:
                WalkingBehavior();
                UpdateWalkIdleTimer();
                break;
            case State.Idle:
                IdleBehavior();
                UpdateWalkIdleTimer();
                break;
            case State.Flee:
                FleeBehavior(toPlayer);
                break;
        }
    }

    void WalkingBehavior()
    {
        transform.position += walkDirection * moveSpeed * Time.deltaTime;
        Quaternion tr = Quaternion.FromToRotation(Vector3.right, walkDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);
    }

    void IdleBehavior()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }

    void FleeBehavior(Vector3 toPlayer)
    {
        Vector3 fleeDir = -toPlayer.normalized;
        fleeDir.y = 0;
        transform.position += fleeDir * fleeSpeed * Time.deltaTime;
        Quaternion tr = Quaternion.FromToRotation(Vector3.right, fleeDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);
    }

    void EnterFleeState()
    {
        currentState = State.Flee;
        fleeTimer = fleeDuration;
        float playerRun = playerCtl != null ? playerCtl.runSpeed : moveSpeed;
        fleeSpeed = playerRun * fleeSpeedMultiplier;
    }

    void UpdateWalkIdleTimer()
    {
        stateTimer += Time.deltaTime;
        if (stateTimer >= currentStateDuration)
        {
            if (currentState == State.Walking) SetIdleState();
            else if (currentState == State.Idle) SetRandomWalkState();
        }
    }

    void SetRandomWalkState()
    {
        currentState = State.Walking;
        stateTimer = 0f;
        currentStateDuration = Random.Range(minWalkDuration, maxWalkDuration);
        float ang = Random.Range(0f, 360f);
        walkDirection = new Vector3(Mathf.Cos(ang * Mathf.Deg2Rad), 0, Mathf.Sin(ang * Mathf.Deg2Rad));
    }

    void SetIdleState()
    {
        currentState = State.Idle;
        stateTimer = 0f;
        currentStateDuration = Random.Range(minIdleDuration, maxIdleDuration);
    }
}
