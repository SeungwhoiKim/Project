using UnityEngine;

public class GateController : MonoBehaviour
{
    [Header("Gate Rotation")]
    private Quaternion closedRotation;
    private Quaternion openRotation;
    public float openAngle = 90f;
    public float openSpeed = 2f;

    private bool isOpen = false;

    void Start()
    {
        closedRotation = transform.rotation;
        openRotation = closedRotation * Quaternion.Euler(0f, openAngle, 0f);

        var col = GetComponent<Collider>();
        if (col == null)
            Debug.LogWarning("GateController: add collider and check isTriggered.");
        else
            col.isTrigger = true;
    }

    void Update()
    {
        if (isOpen)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                openRotation,
                openSpeed * Time.deltaTime
            );
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (isOpen) return;
        if (!other.CompareTag("Player")) return;

        if (KeyManager.Instance != null && KeyManager.Instance.HasKey)
        {
            isOpen = true;
        }
        else
        {
            Debug.Log("GateController: No key, cannot open the gate.");
        }
    }
}
