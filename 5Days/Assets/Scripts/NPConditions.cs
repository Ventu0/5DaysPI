using NUnit.Framework;
using UnityEngine;  

public class NPConditions : MonoBehaviour
{
    [SerializeField] NPC actualNPC;
    [SerializeField] DialogueOptions actualOptions;
    [SerializeField] Sleep sleepScript;
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

    }

    public void DoAction(NPC whichNPC, DialogueOptions options)
    {
        actualNPC = whichNPC;
        actualOptions = options;

        if (options.needMoney)
        {
            WasteMoney();
        }

        if(options.canSleep)
        {
            if(CheckIfHasText(actualOptions.yesDialogue))
            {
                actualNPC.ResetNPC();
            }
            Sleep.instance.SleepForTheDay();
        }
    }
    void WasteMoney()
    {
        int money = PlayerMoney.money;
        if (money >= actualOptions.moneyAmount)
        {
            string[] yesLines = actualOptions.yesDialogue.lines;
            Sprite[] yesFaces = actualOptions.yesDialogue.faces;

            money -= actualOptions.moneyAmount;
            if (!CheckIfHasText(actualOptions.yesDialogue))
            {
                actualNPC.ResetNPC();
                return;
            }

            actualNPC.Falar(yesLines, yesFaces);
        }
        else
        {
            string[] noMoneyLines = actualOptions.noDialogue.lines;
            Sprite[] noMoneyFaces = actualOptions.noDialogue.faces;
            if (!CheckIfHasText(actualOptions.notEnoughMoneyDialogues))
            {
                actualNPC.ResetNPC();
                return;
            }
            actualNPC.Falar(noMoneyLines, noMoneyFaces);
        }
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
