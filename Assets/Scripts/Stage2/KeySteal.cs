using UnityEngine;

public class KeySteal : MonoBehaviour
{
    [Header("References")]
    public Transform guard;
    public Transform player;
    public GameObject promptUI;

    [Header("Steal Settings")]
    public float stealDistance = 2f;
    public float guardFOV = 90f;
    public KeyCode stealKey = KeyCode.E;

    void Start()
    {
        if (player == null && GameObject.FindWithTag("Player") != null)
            player = GameObject.FindWithTag("Player").transform;

        promptUI.SetActive(false);
    }

    void Update()
    {
        if (KeyManager.Instance.HasKey)
        {
            if (promptUI.activeSelf)
                promptUI.SetActive(false);
            enabled = false;
            return;
        }

        float dist = Vector3.Distance(player.position, transform.position);
        if (dist <= stealDistance)
        {
            Vector3 toPlayer = (player.position - guard.position).normalized;
            float angle = Vector3.Angle(guard.forward, toPlayer);
            if (angle > guardFOV * 0.5f)
            {
                if (!promptUI.activeSelf)
                    promptUI.SetActive(true);

                if (Input.GetKeyDown(stealKey))
                {
                    KeyManager.Instance.AcquireKey();
                    promptUI.SetActive(false);
                    Destroy(gameObject);
                }
                return;
            }
        }

        if (promptUI.activeSelf)
            promptUI.SetActive(false);
    }
}
