using UnityEngine;
using UnityEngine.SceneManagement;
public class NPConditions : MonoBehaviour
{
    [SerializeField] NPC currentNPC;
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
        currentNPC = whichNPC;
        currentOption = options;

        if (!options.needsSomething)
        {
            currentNPC.Falar(currentOption.yesDialogue.lines, currentOption.yesDialogue.faces);
            return; 
        }
        if (options.needMoney)
        {
            WasteMoney();
        }
        if (options.needSleep && !options.needMoney)
        {
            print("nao preciso de dinheiro");
            SleepToDay();
        }
        
    }
    void SleepToDay()
    {
        if (!canSleep)
        {
            return;
        }
        if (!CheckIfHasText(currentOption.yesDialogue))
        {
            currentNPC.ResetNPC();
        }
        Sleep.instance.SleepForTheDay();
        currentNPC.canBeInteracted = false;
        canSleep = false;
    }
    void WasteMoney()
    { 
        if(currentOption.alreadyPayed)
        {
            currentNPC.Falar(currentOption.yesPayedDialogue.lines, currentOption.yesPayedDialogue.faces);
            return;
        }
        int money = PlayerMoney.money;

        bool condition = money >= currentOption.moneyAmount;
        if(condition) PlayerMoney.instance.AddMoneyNoAnimation(-currentOption.moneyAmount);
        print("Tem dinheiro: " + condition);
        DialogueArrays dialogueToUse = condition ? currentOption.yesDialogue : currentOption.notEnoughMoneyDialogues;

        string[] lines = dialogueToUse.lines;
        Sprite[] sprites = dialogueToUse.faces;

        if (!CheckIfHasText(dialogueToUse) && !currentOption.needSleep) //só ativa se nao for dormir
        {
            Debug.LogWarning("O dialogo selecionado nao tem texto. Dialogo: " + dialogueToUse);
            return;
        }

        if (!currentOption.needToPayAgain)
        {
            currentNPC.yesOrNo.conditionMet = condition;
            currentOption.alreadyPayed = condition;
        }

        if (currentOption.needSleep && condition)
        {
            print("durma");
            SleepToDay();
            return;
        }

        

        if(condition) 
            currentNPC.ChangeOriginalDialogue(currentOption.alreadyPayedDialogue);
        currentNPC.Falar(lines, sprites);
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
