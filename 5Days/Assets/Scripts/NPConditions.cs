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
            int money = PlayerMoney.money;
            if (money >= options.moneyAmount)
            {
                money -= options.moneyAmount;
                whichNPC.Falar(options.yesLines, options.yesFaces);
            }
            else
                whichNPC.Falar(options.notEnoughMoneyText, options.notEnoughMoneyFaces);
        }
        else if(options.canSleep)
        {
            Sleep();
            whichNPC.Falar(options.yesLines, options.yesFaces);
        }
    }
    public void Sleep()
    {

    }
}
