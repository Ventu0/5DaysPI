using UnityEngine;
public class MainSoundtrack : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource audioSource;

    [Header("Sounds")]
    [SerializeField] AudioClip[] mainSoundtracks;
    [SerializeField] AudioClip[] battleSoundtracks;

    public static MainSoundtrack instance;
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
    private void Start()
    {
        ChooseRandomMusic();
    }
    void ChooseRandomMusic()
    {
        int index = Random.Range(0, mainSoundtracks.Length);
        AudioClip clip = mainSoundtracks[index];
        audioSource.clip = mainSoundtracks[index];
        audioSource.Play();
    }
    public void PlayOneShot(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
    
    public void PlayMain(AudioClip soundTrack)
    {
        audioSource.clip = soundTrack;
        audioSource.Play();
    }
    public void TurnVolumeUpDown(float volume = 0.5f)
    {
        audioSource.volume = volume;
    }
}
