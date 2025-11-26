using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;

public class PlayTimeline : MonoBehaviour
{
    [SerializeField] GameObject timelineObj;
    [SerializeField] UnityEvent onTimelineEnd;
    [SerializeField] PlayableDirector timeline;
    void Start()
    {
        timelineObj.SetActive(false);
    }
    public void StartTimeline()
    {
        print("Starting timeline");
        DiaENoite dayNight = DiaENoite.instance;

        if (Player.instance != null)
            Player.instance.isGamePaused = true;

        if (PauseMenuController.instance != null)
            PauseMenuController.instance.canPause = false;

        if (dayNight != null)
        {
            dayNight.relogioScript.SetActive(false);
            dayNight.isPaused = true;
        }
        QuestController.instance.SetAllActive(false);
        PlayerMoney.instance.SetActive(false);

        timelineObj.SetActive(true);
        timeline.Play();
    }
    public void OnTimelineEnd()
    {
        onTimelineEnd?.Invoke();
        timelineObj.SetActive(false);
    }
}
