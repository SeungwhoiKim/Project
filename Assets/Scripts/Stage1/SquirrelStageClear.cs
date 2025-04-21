using UnityEngine;
using UnityEngine.SceneManagement;

public class SquirrelStageClear : MonoBehaviour
{
    public void Interact()
    {
        Debug.Log("Stage Clear!");

        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
    }
}
