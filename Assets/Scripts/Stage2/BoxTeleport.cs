using UnityEngine;

public class BoxTeleport : MonoBehaviour
{
    [Header("Teleport Settings")]
    public Transform teleportTarget;
    public float interactionDistance = 2f;
    public LayerMask wallLayer;
    public float wallDetectThreshold = 0.1f;
    public KeyCode teleportKey = KeyCode.E;

    Transform player;
    Collider boxCollider;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        boxCollider = GetComponent<Collider>();
        if (teleportTarget == null)
            Debug.LogError("BoxTeleport: teleportTarget needed!");
    }

    void Update()
    {
        if (player == null || teleportTarget == null)
            return;

        if (Vector3.Distance(player.position, transform.position) > interactionDistance)
            return;

        if (!IsBoxAgainstWall())
            return;

        if (Input.GetKeyDown(teleportKey))
        {
            var cc = player.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            player.position = teleportTarget.position;
            player.rotation = teleportTarget.rotation;

            if (cc != null) cc.enabled = true;

            Debug.Log("Player teleported past the wall!");
        }
    }

    bool IsBoxAgainstWall()
    {
        Vector3 center = boxCollider.bounds.center;
        Vector3 extents = boxCollider.bounds.extents;
        Vector3[] dirs = {
            Vector3.forward, Vector3.back,
            Vector3.left, Vector3.right
        };

        foreach (var dir in dirs)
        {
            Vector3 origin = center + dir * (extents.magnitude * 0.5f);
            if (Physics.Raycast(origin, dir, extents.magnitude + wallDetectThreshold, wallLayer))
                return true;
        }

        return false;
    }

    void OnDrawGizmosSelected()
    {
        if (boxCollider == null) return;
        Gizmos.color = Color.cyan;
        Vector3 center = boxCollider.bounds.center;
        Vector3 extents = boxCollider.bounds.extents;
        Vector3[] dirs = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };
        foreach (var dir in dirs)
        {
            Vector3 origin = center + dir * (extents.magnitude * 0.5f);
            Gizmos.DrawLine(origin, origin + dir * (extents.magnitude + wallDetectThreshold));
        }
    }
}
