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
            if (yesLines.Length == 0 && yesFaces.Length == 0)
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
            if (actualOptions.notEnoughMoneyDialogues.lines.Length == 0 && actualOptions.notEnoughMoneyDialogues.faces.Length == 0)
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
        return false; //fazendo isso daqui
    }
}
