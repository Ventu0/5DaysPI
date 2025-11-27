using UnityEngine;
using UnityEngine.Playables;
using System;
public class Sleep : MonoBehaviour
{
    public bool resetPlayerPosOnSleep = true;
    [SerializeField] string bedScene;
    [SerializeField] Vector2 playerNewLastSavedPos;
    [SerializeField] NPC[] npc;
    [SerializeField] bool needsToBeNightToSleep = false;
    [Header("Cutscene de Dormir")]
    [SerializeField] PlayableDirector director;
    [SerializeField] GameObject cutscene;

    Action onCutsceneEnd;
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
        pauseMenu = PauseMenuController.instance;
        player = Player.instance;
        dayNight = DiaENoite.instance;

        cutscene.SetActive(false);

        for (int i = 0; i < npc.Length; i++)
        {
            int index = i;
            dayNight.onNightStart += () => npc[index].canBeInteracted = true;
        }
        director.stopped += OnTimelineStopped;
        if(dayNight.relogioScript.GetCurrentHour() >= dayNight.horarioDaNoite && needsToBeNightToSleep)
        {
            print("horario atual: " + dayNight.relogioScript.GetCurrentHour());
            for(int i = 0; i < npc.Length; i++)
            {
                npc[i].canBeInteracted = true;
            }
        }
    }
    public void SleepForTheDay(bool SetNewSavedPos = false, Action onCutsceneEnd = null)
    {
        this.onCutsceneEnd = onCutsceneEnd;
        dayNight.ResetTime();
        dayNight.relogioScript.NextDay();
        if (SetNewSavedPos)
            Player.instance.SavePosition(playerNewLastSavedPos);

        if (PlayerPartyController.instance != null) PlayerPartyController.instance.CurarTodos();

        Player.instance.isGamePaused = true;
        StartingFade();
        PausePlayer(false);
        SavePlayerBed();
    }
    public void SavePlayerBed()
    {
        player.SaveBedPos();
        player.SaveBedScene(bedScene);
    }
    void StartingFade() => FadeController.instance.FadeInForHowMuchTime(1.5f, StartCutscene);
    void StartCutscene()
    {
        
        cutscene.SetActive(true);
        director.Play();
    }
    void OnTimelineStopped(PlayableDirector director) => FadeController.instance.FadeInForHowMuchTime(2, CutsceneStop);
    void CutsceneStop()
    {
        Player.instance.isGamePaused = false;
        onCutsceneEnd?.Invoke();
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
