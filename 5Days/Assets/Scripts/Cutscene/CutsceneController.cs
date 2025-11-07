using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class CutsceneController : MonoBehaviour
{
    [SerializeField] GameObject book;
    [SerializeField] GameObject dontDestroyParent;
    public PlayableDirector director;
    [SerializeField] UnityEvent onInputReceived;
    public AudioSource whisperSound;
    public bool waitingInput = false;
    public static CutsceneController instance;
    private void Awake()
    {
        book.gameObject.SetActive(false);
        int cutsceneEnded = PlayerPrefs.GetInt("CutsceneEnded");
        if(cutsceneEnded == 1)
        {
            book.gameObject.SetActive(true);
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
        Player.instance.canTalk = false;
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

        QuestController.instance.SetQuestWithAnimation("Fale com o chefe da vila sobre o ocorrido");
        SceneManager.LoadScene("CasaDianas");

        PlayerPrefs.SetInt("CutsceneEnded", 1);
        PlayerPrefs.SetInt("FirstQuest", 0);

        PauseMenuController.instance.canPause = true;
        MainSoundtrack.instance.ChooseRandomMainSoundtrack();

        doors.InteractDoors(true);
        doors.florestaCollider.enabled = false;
        dayScript.PauseTime(false); //reseta o tempo
        dayScript.ResetTime();

        player.transform.position = Vector3.zero; //redefine posições do player
        player.gameObject.SetActive(true);
        player.canMove = true;
        player.canTalk = true;
        Sleep.instance.SavePlayerBed();

        Destroy(dontDestroyParent);
    }
}