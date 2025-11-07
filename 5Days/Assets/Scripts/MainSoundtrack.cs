using UnityEngine;
using System.Collections;
public class MainSoundtrack : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] float mainVolume;

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
        int cutsceneEnded = PlayerPrefs.GetInt("CutsceneEnded", 0);
        if (cutsceneEnded == 0) return;

        ChooseRandomMainSoundtrack(); //cuidado vital
    }
    public void ChooseRandomMainSoundtrack()
    {
        int index = Random.Range(0, mainSoundtracks.Length);
        AudioClip clip = mainSoundtracks[index];
        PlayMain(clip);
    }
    public void ChooseRandomBattleSoundtrack()
    {
        int index = Random.Range(0, battleSoundtracks.Length);
        AudioClip clip = battleSoundtracks[index];
        PlayMain(clip);
    }
    public void PlayOneShot(AudioClip clip, float volume = 1)
    {
        audioSource.PlayOneShot(clip, volume);
    }
    
    public void PlayMain(AudioClip soundTrack)
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        audioSource.clip = soundTrack;
        audioSource.Play();
    }
    IEnumerator PlayNextAudio(AudioClip clip, AudioClip newClip)
    {
        audioSource.clip = clip;
        audioSource.Play();
        yield return new WaitUntil(() => !audioSource.isPlaying);
        audioSource.clip = newClip;
    }
    public void TurnVolumeUpDown(float volume = 0.1f)
    {
        StartCoroutine(VolumeLerp(volume));
    }
    IEnumerator VolumeLerp(float newVolume, float duration = 0.5f)
    {
        float iterador = 0;
        while (iterador < duration)
        {
            audioSource.volume = Mathf.Lerp(audioSource.volume, newVolume, iterador / duration);
            iterador += Time.deltaTime;
            yield return null;
        }
    }
}
