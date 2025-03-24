using UnityEngine;

public class SquirrelAI : MonoBehaviour
{
    // 이동 및 회전 관련 속성
    public float moveSpeed = 1.0f;
    public float rotationSpeed = 5f;  // 회전 보간 계수

    // 상태 지속 시간 범위 (Walking, Idle)
    public float minWalkDuration = 3f;
    public float maxWalkDuration = 6f;
    public float minIdleDuration = 2f;
    public float maxIdleDuration = 4f;

    // Idle 상태에서 추가 회전 여부
    public bool lookAroundDuringIdle = true;

    // 상태 정의
    private enum State { Walking, Idle }
    private State currentState = State.Walking;

    // 상태 타이머 및 현재 상태 지속 시간
    private float stateTimer = 0f;
    private float currentStateDuration;

    // Walking 상태에서 사용할 이동 방향 (xz 평면)
    private Vector3 walkDirection;

    // Idle 상태로 전환될 때 저장할 회전 베이스라인과 Idle 동안의 누적 y회전
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
        else // Idle 상태
        {
            IdleBehavior();
        }

        // 상태 지속 시간이 지나면 전환
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

    // 걷기 상태: 이동하고, 이동 방향에 맞게 머리(로컬 x축)를 회전시킵니다.
    void WalkingBehavior()
    {
        // 이동 처리
        transform.position += walkDirection * moveSpeed * Time.deltaTime;

        // 모델의 머리 방향(로컬 x축)을 이동 방향과 맞추기 위해 FromToRotation 사용
        Quaternion targetRotation = Quaternion.FromToRotation(Vector3.right, walkDirection);
        // 부드러운 회전 적용
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    // Idle 상태: Idle 전 저장된 회전 베이스라인을 기준으로 y축 회전을 누적
    void IdleBehavior()
    {
        if (lookAroundDuringIdle)
        {
            idleYRotation += rotationSpeed * Time.deltaTime;
            transform.rotation = idleBaseline * Quaternion.Euler(0, idleYRotation, 0);
        }
    }

    // Walking 상태 전환: 랜덤 이동 방향과 지속 시간 설정
    void SetRandomWalkState()
    {
        currentState = State.Walking;
        stateTimer = 0f;
        currentStateDuration = Random.Range(minWalkDuration, maxWalkDuration);

        float randomAngle = Random.Range(0f, 360f);
        // xz 평면에서 랜덤 방향 생성
        walkDirection = new Vector3(Mathf.Cos(randomAngle * Mathf.Deg2Rad), 0, Mathf.Sin(randomAngle * Mathf.Deg2Rad));
    }

    // Idle 상태 전환: 현재 회전을 베이스라인으로 저장
    void SetIdleState()
    {
        currentState = State.Idle;
        stateTimer = 0f;
        currentStateDuration = Random.Range(minIdleDuration, maxIdleDuration);

        // Idle 전의 회전을 베이스라인으로 저장 (머리 방향이 이동 방향과 맞춰진 상태)
        idleBaseline = transform.rotation;
        idleYRotation = 0f;
    }
}
