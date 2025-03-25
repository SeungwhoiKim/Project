using UnityEngine;
using UnityEngine.SceneManagement;

public class SquirrelFOV : MonoBehaviour
{
    public float viewDistance = 10f;
    public float viewAngle = 90f;
    public Transform player;
    public LayerMask detectionMask;
    public Vector3 originOffset = new Vector3(0, 1f, 0);

    void Update()
    {
        if (player == null)
            return;

        Vector3 origin = transform.position + originOffset;
        Vector3 dirToPlayer = player.position - origin;
        float distance = dirToPlayer.magnitude;

        if (distance > viewDistance)
            return;

        float angle = Vector3.Angle(transform.forward, dirToPlayer);
        if (angle > viewAngle * 0.5f)
            return;

        Collider[] hits = Physics.OverlapSphere(origin, viewDistance, detectionMask);

        foreach (Collider hit in hits)
        {
            if (hit.transform == player)
            {
                Debug.Log("Player detected via OverlapSphere.");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                break;
            }
        }
    }
}
