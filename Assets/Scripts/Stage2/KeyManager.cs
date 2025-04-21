using UnityEngine;

public class KeyManager : MonoBehaviour
{
    public static KeyManager Instance { get; private set; }

    public bool HasKey { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void AcquireKey()
    {
        HasKey = true;
        Debug.Log("Key acquired!");
    }
}
