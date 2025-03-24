using UnityEngine;
using UnityEngine.UI;

public class OffScreenIndicator : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("UI indicator")]
    public RectTransform indicator;
    public float borderMargin = 50f;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        if (indicator != null)
            indicator.gameObject.SetActive(false);
    }

    void Update()
    {
        if (target == null || indicator == null)
            return;

        Vector3 screenPos = cam.WorldToScreenPoint(target.position);
        bool isBehind = screenPos.z < 0;

        bool isOnScreen = !isBehind &&
                          screenPos.x >= 0 && screenPos.x <= Screen.width &&
                          screenPos.y >= 0 && screenPos.y <= Screen.height;

        if (isOnScreen)
        {
            indicator.gameObject.SetActive(false);
            return;
        }
        else
        {
            indicator.gameObject.SetActive(true);
        }

        if (isBehind)
        {
            screenPos *= -1;
        }

        Vector3 screenCenter = new Vector3(Screen.width, Screen.height, 0) / 2f;
        Vector3 fromCenterToTarget = screenPos - screenCenter;

        float indicatorX = Mathf.Clamp(screenPos.x, borderMargin, Screen.width - borderMargin);
        float indicatorY = Mathf.Clamp(screenPos.y, borderMargin, Screen.height - borderMargin);
        indicator.position = new Vector3(indicatorX, indicatorY, 0);

        float angle = Mathf.Atan2(fromCenterToTarget.y, fromCenterToTarget.x) * Mathf.Rad2Deg;
        indicator.rotation = Quaternion.Euler(0, 0, angle - 90);
    }
}
