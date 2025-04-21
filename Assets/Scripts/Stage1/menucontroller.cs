using UnityEngine;
using UnityEngine.SceneManagement;

public class Control : MonoBehaviour
{
    public void NextScene()
    {
        SceneManager.LoadScene("island");
    }

    public void doExitGame() {
    Application.Quit();
    }
}