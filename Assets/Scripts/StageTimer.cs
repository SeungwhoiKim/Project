using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageTimer : MonoBehaviour
{
    public float totalTime = 180f;
    private float timeRemaining;

    public Text timerText;

    void Start()
    {
        timeRemaining = totalTime;
    }

    void Update()
    {
        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            if (SquirrelCaptureManager.Instance != null &&
                SquirrelCaptureManager.Instance.CapturedCount < SquirrelCaptureManager.Instance.squirrelTarget)
            {
                Debug.Log("Time's up! Not enough squirrels captured. Restarting stage.");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);
        timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }
}
