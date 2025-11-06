using UnityEngine;
using UnityEngine.SceneManagement;
public class NPConditions : MonoBehaviour
{
    [SerializeField] NPC actualNPC;
    [SerializeField] DialogueOptions currentOption;
    public bool canSleep = true;
    public static NPConditions instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        if(DiaENoite.instance != null)
            DiaENoite.instance.onNightStart += () => canSleep = true;
    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void DoAction(NPC whichNPC, DialogueOptions options)
    {
        actualNPC = whichNPC;
        currentOption = options;

        if (options.needMoney)
        {
            WasteMoney();
        }
        if(options.needSleep)
        {
            if (!canSleep)
            {
                return; 
            }
            //se quiser colocar uma cutscene de dormir aqui, colocar aqui
            if (!CheckIfHasText(currentOption.yesDialogue))
            {
                actualNPC.ResetNPC();
            }
            Sleep.instance.SleepForTheDay();
            whichNPC.canBeInteracted = false;
            canSleep = false;
        }
    }
    void WasteMoney()
    {
        int money = PlayerMoney.money;

        bool condition = money >= currentOption.moneyAmount;
        if(condition) money -= currentOption.moneyAmount;

        DialogueArrays dialogueToUse = condition ? currentOption.yesDialogue : currentOption.notEnoughMoneyDialogues;

        string[] lines = dialogueToUse.lines;
        Sprite[] sprites = dialogueToUse.faces;

        if (!CheckIfHasText(dialogueToUse))
        {
            Debug.LogWarning("O dialogo selecionado nao tem texto. Dialogo: " + dialogueToUse);
            return;
        }
        actualNPC.yesOrNo.conditionMet = condition;
        actualNPC.Falar(lines, sprites);
    }
    bool CheckIfHasText(DialogueArrays dialogue)
    {
        string[] dialogueLines = dialogue.lines;
        Sprite[] dialogueFaces = dialogue.faces;
        
        if(dialogueLines.Length == 0 && dialogueFaces.Length == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
