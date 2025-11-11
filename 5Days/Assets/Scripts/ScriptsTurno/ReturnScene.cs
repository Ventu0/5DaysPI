using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using JetBrains.Annotations;
public class ReturnScene : MonoBehaviour
{
    [Header("Configurações animation")]
    [SerializeField] Button loseButton;
    [SerializeField] float duration = 1.5f;
    [SerializeField] float characterSpeed = 3;
    [SerializeField] string sceneName;
    [SerializeField] Animator fadeAnimation;
    bool lost;
    public static ReturnScene instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        loseButton.onClick.AddListener(ReturnSceneBTN);
        fadeAnimation.gameObject.SetActive(false);
    }
    public void ReturnSceneBTN()
    {   
        PersistentObject persistent = PersistentObject.instance;
        int isHardcore = PlayerPrefs.GetInt("HardcoreMode", 0);
        if(lost && isHardcore == 1)
        {
            persistent.StartCoroutine(persistent.HardcoreModeLost());
        }
        else if(lost)   persistent.StartCoroutine(persistent.FadeSequence());

        TurnModeManager turnModeManager = TurnModeManager.instance;
        turnModeManager.MaintainStatus();
        turnModeManager.currentFightingEnemy.battleStarted = false;

        Player.instance.canMove = true;
        QuestController.instance.SetAllActive(true);
        DiaENoite dayScript = DiaENoite.instance;
        if (dayScript.clockUI != null) dayScript.clockUI.SetActive(true);
        dayScript.PauseTime(false);

        SceneManager.UnloadSceneAsync(sceneName);
        MainSoundtrack.instance.ChooseRandomMainSoundtrack();
        SceneTimeController.instance.onPauseGame?.Invoke();
        PauseMenuController.instance.canPause = true;

        Time.timeScale = 1f;
    }


    public void AddLoseMethod() => lost = true;
    public IEnumerator RunAnimation(BasePersonagem[] characters)
    {
        float iterador = 0;
        fadeAnimation.gameObject.SetActive(true);
        FadeController.instance.FadeInForHowMuchTime(2f);
        InteractButtonsController.instance.menu.SetActive(false);
        while (iterador < duration)
        {
            for(int i = 0; i < characters.Length; i++)
            {
                Transform characterTransform = characters[i].transform;
                characters[i].GetComponent<SpriteRenderer>().flipX = true;
                iterador += Time.deltaTime;
                characterTransform.Translate(Vector3.left * characterSpeed * iterador / duration);
                yield return null;
            }
        }
        ReturnSceneBTN();
    }
}