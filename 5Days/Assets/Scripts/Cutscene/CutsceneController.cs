using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;
public class CutsceneController : MonoBehaviour
{
    [SerializeField] GameObject dontDestroyParent;
    public PlayableDirector director;
    [SerializeField] UnityEvent onInputReceived;
    public bool waitingInput = false;
    private void Awake()
    {
        director.Play();
        int checkIfCutsceneWasPlayed = PlayerPrefs.GetInt("AlreadyPlayedCutscene", 0);
        if (checkIfCutsceneWasPlayed == 1) //se a cutscene ja foi tocada
        {
            Destroy(dontDestroyParent);
            return;
        }
        DontDestroyOnLoad(dontDestroyParent);
    }

    void Start()
    {
        
    }
    public void OnSceneChange()
    {
        Doors doors = Doors.instance;
        DiaENoite diaENoite = DiaENoite.instance;
        diaENoite.directionalLight.gameObject.SetActive(false);
        diaENoite.directionalLight.gameObject.SetActive(true);
    }
    public void Wait(bool waitInput = true)
    {
        if (waitingInput)
        waitingInput = true;

        director.Pause();
    }
    void Update()
    {
       if(waitingInput && Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
       {
           onInputReceived.Invoke();
       }
    }
    public void EndCutscene()
    {
        PlayerPrefs.SetInt("AlreadyPlayedCutscene", 1);
        Destroy(dontDestroyParent);
    }
}
