using UnityEngine;

public class BabyFoxGoal : MonoBehaviour
{
    [Header("Goal Settings")]
    public Transform babyFox;
    public float clearDistance = 2f;

    bool hasCleared = false;

    void Start()
    {
        if (babyFox == null)
        {
            var go = GameObject.FindGameObjectWithTag("BabyFox");
            if (go != null) babyFox = go.transform;
        }
        if (babyFox == null)
            Debug.LogError("BabyFoxGoal: 'babyFox' needed!");
    }

    void Update()
    {
        if (hasCleared || babyFox == null) return;

        float dist = Vector3.Distance(transform.position, babyFox.position);
        if (dist <= clearDistance)
        {
            hasCleared = true;
            Debug.Log("BabyFoxGoal: Goal reached! Triggering Stage Clear.");

            var ui = FindObjectOfType<StageClearUI>();
            if (ui != null)
            {
                ui.TriggerClear();
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(
                    UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1
                );
            }
        }
    }
}
