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
        if (Player.instance != null)
            Player.instance.isGamePaused = true;

        if (PauseMenuController.instance != null)
            PauseMenuController.instance.canPause = false;

        if (DiaENoite.instance != null)
            DiaENoite.instance.isPaused = true;

        timelineObj.SetActive(true);
        timeline.Play();
    }
    public void OnTimelineEnd()
    {
        onTimelineEnd?.Invoke();
        timelineObj.SetActive(false);
    }
}
