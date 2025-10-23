using UnityEngine;
using UnityEngine.Playables;

public class Sleep : MonoBehaviour
{
    [SerializeField] NPC[] npc;

    [Header("Cutscene de Dormir")]
    [SerializeField] PlayableDirector director;
    [SerializeField] GameObject cutscene;

    //instancias
    PauseMenuController pauseMenu;
    DiaENoite dayNight;
    Player player;

    public static Sleep instance;
    private void Awake()
    {   
        instance = this;
    }
    void Start()
    {
        cutscene.SetActive(false);

        for (int i = 0; i < npc.Length; i++)
        {
            int index = i;
            DiaENoite.instance.onNightStart += () => npc[index].canBeInteracted = true;
        }
        director.stopped += OnTimelineStopped;

        pauseMenu = PauseMenuController.instance;
        player = Player.instance;
        dayNight = DiaENoite.instance;
    }
    public void SleepForTheDay()
    {
        Player player = Player.instance;
        dayNight.ResetTime();
        dayNight.relogioScript.NextDay();

        StartingFade();
        player.SaveBedPos();
        player.SaveBedScene();
    }
    void StartingFade() => FadeController.instance.FadeInForHowMuchTime(1.5f, StartCutscene);
    void StartCutscene()
    {
        PausePlayer(false);
        cutscene.SetActive(true);
        director.Play();
    }
    void OnTimelineStopped(PlayableDirector director) => FadeController.instance.FadeInForHowMuchTime(2, CutsceneStop);
    void CutsceneStop()
    {
        PausePlayer(true);
        cutscene.SetActive(false);
    }
    void PausePlayer(bool pause)
    {
        if (player != null)
            player.canMove = pause;

        if (pauseMenu != null)
            pauseMenu.canPause = pause;

        if (dayNight != null)
            dayNight.isPaused = !pause;
    }
}
