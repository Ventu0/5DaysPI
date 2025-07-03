using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System;
public class CutsceneController : MonoBehaviour
{
    [SerializeField] GameObject dontDestroyParent;
    public PlayableDirector director;
    [SerializeField] UnityEvent onInputReceived;
    public bool waitingInput = false;
    public static CutsceneController instance;
    private void Awake()
    {
        if(CutsceneStatus.cutsceneEnded)
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
        Player player = Player.instance;
        QuestController.instance.SetQuestText("Fale com o chefe da vila sobre o ocorrido");
        CutsceneStatus.cutsceneEnded = true;
        SceneManager.LoadScene("CasaDianas");
        DiaENoite.instance.ResetTime();
        player.transform.position = Vector3.zero;
        player.gameObject.SetActive(true);
        player.canMove = true;
        Doors.instance.InteractDoors(true);
        Destroy(dontDestroyParent);
    }
}
[Serializable]
public class CutsceneStatus
{
    public static bool cutsceneStarted = false;
    public static bool cutsceneEnded = false;
}
