using UnityEngine;
using UnityEngine.UI;

public class SquirrelCounterUI : MonoBehaviour
{
    public Text counterText;

    void Update()
    {
        if (SquirrelCaptureManager.Instance != null && counterText != null)
        {
            counterText.text = SquirrelCaptureManager.Instance.CapturedCount.ToString()
                                 + " / "
                                 + SquirrelCaptureManager.Instance.squirrelTarget.ToString();
        }
    }
}
