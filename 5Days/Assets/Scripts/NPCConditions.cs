using UnityEngine;
using UnityEngine.SceneManagement;
public class NPCConditions : MonoBehaviour
{

    public static NPCConditions instance;
    private void Awake()
    {
        if (instance == null)
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
    public bool CheckIfCanPay(int amount)
    {
        int money = PlayerMoney.money;

        if (money > 0)
            return true;
        else
            return false;

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
