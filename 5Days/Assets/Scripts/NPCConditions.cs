using UnityEngine;
using UnityEngine.SceneManagement;
public class NPCConditions : MonoBehaviour
{
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    public void PlayerPay(int amount)
    {
        int money = PlayerMoney.money;
        if (money >= amount)
        {
            money -= amount;
        }
        else print("SemMoney");
    }
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
