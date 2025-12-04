using UnityEngine;
using UnityEngine.Playables;
using System.Collections;
using Unity.VisualScripting;

public class CheckBook : MonoBehaviour, IInteractable
{
    [Header("References")]
    [SerializeField] GameObject cutsceneObject;
    [SerializeField] GameObject closeCutscene;
    [SerializeField] PlayableDirector timeline;
    [SerializeField] SkipCutscene cutsceneSkipper;

    [Header("Config")]
    [SerializeField] float skipToTime = 11.26f;
    [SerializeField] bool waitForAnimation = false;
    [SerializeField] GameObject animationObject;
    [SerializeField] float animationTime;

    [Header("Read-Only")]
    [SerializeField] bool animationEnded;
    bool oneTime; //porque ta repetindo e dando bug

    Player player;
    PauseMenuController pauseMenu;
    DiaENoite dayNight;
    ChatController chatController;
    void Awake()
    {
        if (animationObject != null) animationObject.SetActive(false);
        cutsceneObject.SetActive(false);
        closeCutscene.SetActive(false);
        player = Player.instance;
        pauseMenu = PauseMenuController.instance;
        dayNight = DiaENoite.instance;
        chatController = ChatController.instance;
    }
    public void OnExitRange()
    {
        chatController.interactBTN.gameObject.SetActive(false);
        chatController.interactBTN.onClick.RemoveAllListeners();
    }
    public void OnReachRange()
    {
        chatController.interactBTN.gameObject.SetActive(true);
        chatController.interactBTN.onClick.AddListener(() => Interact());
    }
    public void Interact()
    {
        print("interact");
        if (player != null)
            player.canMove = false;

        if (pauseMenu != null)
            pauseMenu.canPause = false;

        if (dayNight != null)
            dayNight.isPaused = true;
        DiaENoite.instance.isPaused = true;

        if (waitForAnimation && !animationEnded)
        {
            if (!oneTime)
            StartCoroutine(StartAnimation());
            return;
        }
        
        cutsceneObject.SetActive(true);
        cutsceneSkipper.finished = false;
        print("ativando cutscene do livro");
        cutsceneSkipper.canSkipCutscene = true;
        timeline.Play();
    }
    IEnumerator StartAnimation()
    {
        oneTime = true;
        animationObject.SetActive(true);
        yield return new WaitForSeconds(animationTime);
        animationObject.SetActive(false);
        animationEnded = true;
        OnReachRange();
    }
    public void CloseCutscene()
    {
        if (player != null)
            player.canMove = true;

        if (pauseMenu != null)
            pauseMenu.canPause = true;

        if (dayNight != null)
            dayNight.isPaused = false;

        cutsceneObject.SetActive(false);
    }
    public void Skip()
    {
        cutsceneSkipper.canSkipCutscene = false;
        timeline.time = skipToTime;
        timeline.Evaluate();
        closeCutscene.SetActive(true);
        StopCutscene();
    }
    public void StopCutscene()
    {
        print("parando cutscene");
        closeCutscene.SetActive(true);
        cutsceneSkipper.CompletedSkip();
        timeline.Pause();
    }
}
