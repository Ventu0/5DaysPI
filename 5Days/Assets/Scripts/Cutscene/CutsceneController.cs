using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class CutsceneController : MonoBehaviour
{
    [SerializeField] GameObject dontDestroyParent;
    public PlayableDirector director;
    [SerializeField] UnityEvent onInputReceived;
    public bool waitingInput = false;
    public static CutsceneController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(dontDestroyParent);
        }
            director.Play();
        //int checkIfCutsceneWasPlayed = PlayerPrefs.GetInt("AlreadyPlayedCutscene", 0);
        //if (checkIfCutsceneWasPlayed == 1) //se a cutscene ja foi tocada
        //{
        //    Destroy(dontDestroyParent);
        //    return;
        //}
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
        if (waitInput)
        waitingInput = true;

        director.Pause();
    }
    void Update()
    {
       if(waitingInput && Input.GetKeyDown(KeyCode.E) || waitingInput && Input.GetMouseButtonDown(0))
       {
           onInputReceived.Invoke();
       }
    }
    public void EndCutscene()
    {
        //PlayerPrefs.SetInt("AlreadyPlayedCutscene", 1);
        QuestController.instance.SetQuestText("Fale com o chefe da vila sobre o ocorrido");
        SceneManager.LoadScene("CasaDianas");
        DiaENoite.instance.ResetTime();
        //Player.instance.transform.position = new Vector3.zero;
        dontDestroyParent.SetActive(false);
    }
}
