using UnityEngine;
using TMPro;
using System.Collections;
public class PlayerMoney : MonoBehaviour
{
    [SerializeField] bool debugMode = false;
    [Space(10)] 
    [SerializeField] GameObject moedaGira;
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] TextMeshProUGUI moneyToAddText;
    public static int money { get; private set; } = 0;
    public static PlayerMoney instance;

    private void Awake()
    {
        int cutsceneEnded = PlayerPrefs.GetInt("CutsceneEnded", 0);
        if (cutsceneEnded == 0 && !debugMode)
        {
            Destroy(transform.root.gameObject);
            return;
        }
        moneyText = GameObject.Find("MoneyText").GetComponent<TextMeshProUGUI>();
        moneyText.text = money.ToString();
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        moneyToAddText.text = "";
        moneyToAddText.gameObject.SetActive(false);
    }

    public void SetActive(bool setActive = true)
    {
        moedaGira.SetActive(setActive);
    }

    public void AddMoneyNoAnimation(int amount)
    {
        money += amount;
        SkipFrame();
    }

    public void SkipFrame()
    {
        moneyText.text = money.ToString();
    }

    public void AddMoney(int amount)
    {
        StartCoroutine(MoneyAnimation(amount));
    }

    IEnumerator MoneyAnimation(int amount)
    {
        moneyToAddText.gameObject.SetActive(true);
        int moneyToAdd = amount;
        int moneyAmount = money;
        int fakeMoney = money;
        moneyToAddText.text = "+" + moneyToAdd.ToString();

        yield return new WaitForSeconds(2);

        while (moneyToAdd != 0 || fakeMoney <= moneyAmount + amount)
        {
            if (moneyToAdd != 0)
            {
                moneyToAdd -= 1;
                moneyToAddText.text = "+" + moneyToAdd.ToString();
            }

            yield return new WaitForSeconds(0.05f);

            if (fakeMoney <= moneyAmount + amount)
            {
                fakeMoney += 1;
                moneyText.text = fakeMoney.ToString();
            }
        }

        money += amount;
        moneyText.text = money.ToString();
        moneyToAddText.GetComponent<Animator>().SetTrigger("Ativar");

        yield return new WaitForSeconds(1);

        moneyToAddText.gameObject.SetActive(false);
    }

    #region ContextMenu
    [ContextMenu("See money value")]
    public void SeeMoney()
    {
        print("money: " + money.ToString());
        moneyText.text = money.ToString();
    }

    [ContextMenu("Add money")]
    public void AddMoney()
    {
        money += 10;
    }
    #endregion
}
