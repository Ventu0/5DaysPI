using UnityEngine;
using TMPro;
using System.Collections;
public class PlayerMoney : MonoBehaviour
{
    public static int money { get; private set; } = 0;
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] TextMeshProUGUI moneyToAddText;
    void Start()
    {
        moneyToAddText.gameObject.SetActive(false);
        AddMoney(100);
    }
    void Update()
    {
        
    }
    public void AddMoney(int amount)
    {
        StartCoroutine(MoneyAnimation(amount));
    }
    IEnumerator MoneyAnimation(int amount)
    {
        moneyToAddText.gameObject.SetActive(true);
        int moneyToAdd = amount;
        moneyToAddText.text = "+" + moneyToAdd.ToString();
        yield return new WaitForSeconds(2);

        while (moneyToAdd != 0)
        {
            moneyToAdd -= 1;
            yield return null;
        }
        while(money != money + amount)
        {
            money += 1;
            yield return null;
        }
        moneyText.text = money.ToString();
        moneyToAddText.gameObject.SetActive(false);
    }
}
