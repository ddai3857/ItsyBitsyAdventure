using UnityEngine;

public class PersistentAudio : MonoBehaviour
{
    // Static reference to ensure only one instance exists game-wide
    public static PersistentAudio instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);
    }
}