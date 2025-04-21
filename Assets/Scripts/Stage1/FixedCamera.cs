using UnityEngine;

public class FixedCamera : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0, 10, -5);

    void LateUpdate()
    {
        if (player != null)
        {
            transform.position = player.position + offset;
            transform.LookAt(player);
        }
    }
}
