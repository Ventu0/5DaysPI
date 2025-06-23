using UnityEngine;

public class SFX : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    public static SFX instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        audioSource = GetComponentInChildren<AudioSource>();
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (audioSource.isPlaying) audioSource.Stop();
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();
    }
}
