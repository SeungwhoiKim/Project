using UnityEngine;

public class SquirrelCapture : MonoBehaviour
{
    public void Interact()
    {
        if (SquirrelCaptureManager.Instance != null)
        {
            SquirrelCaptureManager.Instance.CaptureSquirrel();
        }
        else
        {
            Debug.LogWarning("SquirrelCaptureManager is not found in the scene.");
        }

        Destroy(gameObject);
    }
}
