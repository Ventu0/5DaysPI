using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class CutsceneController : MonoBehaviour
{
    [SerializeField] GameObject dontDestroyParent;
    public PlayableDirector director;
    [SerializeField] UnityEvent onInputReceived;
    public AudioSource whisperSound;
    public bool waitingInput = false;
    public static CutsceneController instance;
    private void Awake()
    {
        int cutsceneEnded = PlayerPrefs.GetInt("CutsceneEnded");
        if(cutsceneEnded == 1)
        {
            Destroy(dontDestroyParent);
            return;
        }
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
        DiaENoite.instance.PauseTime(true);
        PauseMenuController.instance.canPause = false;
    }
    public void OnSceneChange()
    {
        whisperSound.Play();
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
        Player player = Player.instance;
        DiaENoite dayScript = DiaENoite.instance;
        Doors doors = Doors.instance;

        QuestController.instance.SetQuestText("Fale com o chefe da vila sobre o ocorrido");
        SceneManager.sceneLoaded -= doors.OnSceneLoaded;
        SceneManager.LoadScene("CasaDianas");

        PlayerPrefs.SetInt("CutsceneEnded", 1);
        PauseMenuController.instance.canPause = true;

        doors.InteractDoors(true);
        dayScript.PauseTime(false); //reseta o tempo
        dayScript.ResetTime();

        player.transform.position = Vector3.zero; //redefine posições do player
        player.gameObject.SetActive(true);
        player.canMove = true;

        Destroy(dontDestroyParent);
    }
}