using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    [Header("UI References")]
    public GameObject pauseMenuUI;
    public Button resumeButton;
    public Button leaveButton;

    private bool isPaused = false;

    void Awake()
    {
        if (resumeButton == null || leaveButton == null || pauseMenuUI == null)
        {
            Debug.LogError("PauseMenuController: UI references not set in Inspector.");
            enabled = false;
            return;
        }

        resumeButton.onClick.AddListener(Resume);
        leaveButton.onClick.AddListener(LeaveGame);

        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        Debug.Log("Game Resumed");
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        Debug.Log("Game Paused");
    }

    public void LeaveGame()
    {
        Time.timeScale = 1f;
        Debug.Log("Leaving Game...");
        SceneManager.LoadScene("title");
    }
}