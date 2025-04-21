using UnityEngine;
using System.Collections;

public class WireBite : MonoBehaviour
{
    [Header("References")]
    public Transform boxToDrop;
    public GameObject wireVisual;
    public KeyCode biteKey = KeyCode.E;

    [Header("Interaction")]
    public float interactDistance = 2f;
    public bool showPrompt = true;
    public GameObject promptUI;

    [Header("Drop Settings")]
    public float dropSpeed = 2f;
    public float dropDistance = 3f;

    private Transform player;
    private bool hasDropped = false;
    private float droppedSoFar = 0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (promptUI != null) promptUI.SetActive(false);
    }

    void Update()
    {
        if (hasDropped || player == null) return;

        float dist = Vector3.Distance(player.position, transform.position);
        bool inRange = dist <= interactDistance;

        if (showPrompt && promptUI != null)
            promptUI.SetActive(inRange);

        if (inRange && Input.GetKeyDown(biteKey))
        {
            if (wireVisual != null) Destroy(wireVisual);
            else Destroy(gameObject);

            hasDropped = true;
            droppedSoFar = 0f;
            if (promptUI != null) promptUI.SetActive(false);

            StartCoroutine(DropConstantSpeed());
        }
    }

    private IEnumerator DropConstantSpeed()
    {
        Vector3 down = Vector3.down;
        while (droppedSoFar < dropDistance)
        {
            float delta = dropSpeed * Time.deltaTime;
            if (droppedSoFar + delta > dropDistance)
                delta = dropDistance - droppedSoFar;

            boxToDrop.position += down * delta;
            droppedSoFar += delta;
            yield return null;
        }
    }
}
