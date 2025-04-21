using UnityEngine;

public class SquirrelCaptureManager : MonoBehaviour
{
    public static SquirrelCaptureManager Instance;

    public int squirrelTarget = 10;

    private int capturedCount = 0;

    public int CapturedCount
    {
        get { return capturedCount; }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CaptureSquirrel()
    {
        capturedCount++;
        Debug.Log("Squirrel captured! Count: " + capturedCount);

        if (capturedCount >= squirrelTarget)
        {
            Debug.Log("Stage Clear!");
        }
    }
}
