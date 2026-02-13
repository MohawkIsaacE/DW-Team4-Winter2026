using UnityEngine;
using UnityEngine.Audio;

public class audioManager : MonoBehaviour
{
    public static audioManager Instance;

    public AudioSource source;
    public AudioClip throwNoise;
    public AudioClip hitNoise;
    public AudioClip spicyNoise;
    public AudioClip chipCrunch;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        source = GetComponent<AudioSource>();
    }
}

    
