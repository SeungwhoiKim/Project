using UnityEngine;

public class SquirrelAI : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public float rotationSpeed = 5f;

    public float minWalkDuration = 3f;
    public float maxWalkDuration = 6f;
    public float minIdleDuration = 2f;
    public float maxIdleDuration = 4f;

    public bool lookAroundDuringIdle = true;

    private enum State { Walking, Idle }
    private State currentState = State.Walking;

    private float stateTimer = 0f;
    private float currentStateDuration;

    private Vector3 walkDirection;

    private Quaternion idleBaseline;
    private float idleYRotation = 0f;

    void Start()
    {
        SetRandomWalkState();
    }

    void Update()
    {
        stateTimer += Time.deltaTime;

        if (currentState == State.Walking)
        {
            WalkingBehavior();
        }
        else
        {
            IdleBehavior();
        }

        if (stateTimer >= currentStateDuration)
        {
            if (currentState == State.Walking)
            {
                SetIdleState();
            }
            else
            {
                SetRandomWalkState();
            }
        }
    }

    void WalkingBehavior()
    {
        transform.position += walkDirection * moveSpeed * Time.deltaTime;

        Quaternion targetRotation = Quaternion.FromToRotation(Vector3.right, walkDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void IdleBehavior()
    {
        if (lookAroundDuringIdle)
        {
            idleYRotation += rotationSpeed * Time.deltaTime;
            transform.rotation = idleBaseline * Quaternion.Euler(0, idleYRotation, 0);
        }
    }

    void SetRandomWalkState()
    {
        currentState = State.Walking;
        stateTimer = 0f;
        currentStateDuration = Random.Range(minWalkDuration, maxWalkDuration);

        float randomAngle = Random.Range(0f, 360f);
        walkDirection = new Vector3(Mathf.Cos(randomAngle * Mathf.Deg2Rad), 0, Mathf.Sin(randomAngle * Mathf.Deg2Rad));
    }

    void SetIdleState()
    {
        currentState = State.Idle;
        stateTimer = 0f;
        currentStateDuration = Random.Range(minIdleDuration, maxIdleDuration);

        idleBaseline = transform.rotation;
        idleYRotation = 0f;
    }
}
