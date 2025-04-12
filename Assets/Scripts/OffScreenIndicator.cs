using UnityEngine;
using UnityEngine.UI;

public class OffScreenIndicator : MonoBehaviour
{
    [Header("UI Setting")]
    public RectTransform indicator;
    public float borderMargin = 50f;

    [Header("Player and Camera")]
    public Transform player;
    public Camera mainCamera;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    void Update()
    {
        if (player == null || indicator == null)
            return;

        GameObject[] squirrels = GameObject.FindGameObjectsWithTag("Squirrel");
        if (squirrels.Length == 0)
        {
            indicator.gameObject.SetActive(false);
            return;
        }

        Transform closestSquirrel = null;
        float minDist = Mathf.Infinity;
        foreach (GameObject s in squirrels)
        {
            float d = Vector3.Distance(player.position, s.transform.position);
            if (d < minDist)
            {
                minDist = d;
                closestSquirrel = s.transform;
            }
        }

        if (closestSquirrel == null)
        {
            indicator.gameObject.SetActive(false);
            return;
        }

        Vector3 screenPos = mainCamera.WorldToScreenPoint(closestSquirrel.position);

        bool isBehind = screenPos.z < 0;
        if (isBehind)
        {
            screenPos *= -1;
        }

        bool isOnScreen = screenPos.x >= 0 && screenPos.x <= Screen.width &&
                          screenPos.y >= 0 && screenPos.y <= Screen.height && !isBehind;

        if (isOnScreen)
        {
            indicator.gameObject.SetActive(false);
            return;
        }
        else
        {
            indicator.gameObject.SetActive(true);
        }

        float clampedX = Mathf.Clamp(screenPos.x, borderMargin, Screen.width - borderMargin);
        float clampedY = Mathf.Clamp(screenPos.y, borderMargin, Screen.height - borderMargin);
        indicator.position = new Vector3(clampedX, clampedY, 0);

        Vector3 screenCenter = new Vector3(Screen.width, Screen.height, 0) / 2f;
        Vector3 dir = screenPos - screenCenter;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        indicator.rotation = Quaternion.Euler(0, 0, angle - 90);
    }
}
