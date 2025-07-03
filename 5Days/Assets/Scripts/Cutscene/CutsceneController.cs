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
            DontDestroyOnLoad(dontDestroyParent);
        }
        else
        {
            Destroy(dontDestroyParent);
        }
            director.Play();
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
        Destroy(dontDestroyParent);
        DiaENoite.instance.ResetTime();
        Player.instance.transform.position = Vector3.zero;
        Player.instance.gameObject.SetActive(true);
        dontDestroyParent.SetActive(false);
    }
}
