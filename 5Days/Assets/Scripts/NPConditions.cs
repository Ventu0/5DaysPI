using NUnit.Framework;
using System.Linq.Expressions;
using UnityEngine;  

public class NPConditions : MonoBehaviour
{
    [SerializeField] NPC actualNPC;
    [SerializeField] DialogueOptions actualOptions;
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
        DiaENoite.instance.onNightStart += () => canSleep = true;
    }

    public void DoAction(NPC whichNPC, DialogueOptions options)
    {
        actualNPC = whichNPC;
        actualOptions = options;

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
            if (!CheckIfHasText(actualOptions.yesDialogue))
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
        if (money >= actualOptions.moneyAmount)
        {
            string[] yesLines = actualOptions.yesDialogue.lines;
            Sprite[] yesFaces = actualOptions.yesDialogue.faces;

            money -= actualOptions.moneyAmount;
            if (CheckIfHasText(actualOptions.yesDialogue))
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
            if (CheckIfHasText(actualOptions.notEnoughMoneyDialogues))
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
