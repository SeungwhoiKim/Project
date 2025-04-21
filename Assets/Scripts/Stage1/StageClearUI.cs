using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageClearUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject stageClearPanel;
    public Button nextStageButton;

    private bool isShown = false;

    void Start()
    {
        if (stageClearPanel != null)
            stageClearPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (!isShown &&
            SquirrelCaptureManager.Instance != null &&
            SquirrelCaptureManager.Instance.CapturedCount >= SquirrelCaptureManager.Instance.squirrelTarget)
        {
            ShowStageClear();
        }
    }

    public void TriggerClear()
    {
        if (isShown) return;
        ShowStageClear();
    }

    void ShowStageClear()
    {
        isShown = true;

        Time.timeScale = 0f;

        if (stageClearPanel != null)
            stageClearPanel.SetActive(true);

        if (nextStageButton != null)
        {
            nextStageButton.onClick.RemoveAllListeners();
            nextStageButton.onClick.AddListener(OnNextStage);
        }
    }

    void OnNextStage()
    {
        Time.timeScale = 1f;

        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextIndex);
    }
}
