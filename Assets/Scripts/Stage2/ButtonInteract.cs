using UnityEngine;
using System.Collections;

public class ButtonInteract : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public GameObject hintUI;

    [Header("Settings")]
    public float triggerDistance = 2f;
    public float hintDuration = 2f;

    bool isShowing = false;

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (hintUI != null)
            hintUI.SetActive(false);
    }

    void Update()
    {
        if (isShowing || player == null || hintUI == null)
            return;

        Vector3 toButton = transform.position - player.position;
        toButton.y = 0f;

        if (toButton.magnitude <= triggerDistance)
        {
            StartCoroutine(ShowHint());
        }
    }

    IEnumerator ShowHint()
    {
        isShowing = true;
        hintUI.SetActive(true);
        yield return new WaitForSeconds(hintDuration);
        hintUI.SetActive(false);
        isShowing = false;
    }
}
