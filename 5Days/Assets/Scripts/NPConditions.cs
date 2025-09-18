using UnityEngine;

public class NPConditions : MonoBehaviour
{
    [SerializeField] NPC actualNPC;
    [SerializeField] DialogueOptions actualOptions;
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
            Sleep();
        }
    }
    void WasteMoney()
    {
        int money = PlayerMoney.money;
        if (money >= actualOptions.moneyAmount)
        {
            money -= actualOptions.moneyAmount;
            print("gastando dinheiro do player");
            if (actualOptions.yesLines.Length == 0 && actualOptions.yesFaces.Length == 0)
            {
                actualNPC.ResetNPC();
                return;
            }

            actualNPC.Falar(actualOptions.yesLines, actualOptions.yesFaces);
        }
        else
        {
            if(actualOptions.notEnoughMoneyText.Length == 0 && actualOptions.notEnoughMoneyFaces.Length == 0)
            {
                actualNPC.ResetNPC();
                return;
            }
            actualNPC.Falar(actualOptions.notEnoughMoneyText, actualOptions.notEnoughMoneyFaces);
        }
    }
    void Sleep()
    {
            
    }
}
