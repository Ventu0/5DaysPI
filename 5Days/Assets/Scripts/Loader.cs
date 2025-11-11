using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections;

public class Loader : MonoBehaviour
{
    PlayerData playerData;
    public static Loader instance;
    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
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
   
    public void Carregar()
    {
        string caminho = Application.persistentDataPath + "/PlayerData.json";
        playerData = new PlayerData();
        if (File.Exists(caminho))
        {
            string json = File.ReadAllText(caminho);
            playerData = JsonUtility.FromJson<PlayerData>(json);
        }
        Destroy(DeleteSave.instance);
        SceneManager.LoadScene(playerData.activeScene);
    }
    IEnumerator DelayedAddMoney(PlayerMoney money, int amount)
    {
        yield return null; // espera 1 frame
        money.AddMoneyNoAnimation(amount);
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene == null || playerData == null) return;

        if(scene.name != playerData.activeScene)
            return;

        Player player = Player.instance;
        QuestController questController = QuestController.instance;
        PlayerMoney money = PlayerMoney.instance;

        if (player == null) print("Player nulo no Loader");
        player.transform.position = playerData.playerPos;
        player.lastSavedPosition = playerData.playerLastSavedPos;

        questController?.JustSetQuest(playerData.activeQuest);
        if (money == null) print("null");
        StartCoroutine(DelayedAddMoney(money, playerData.money));
        SceneManager.sceneLoaded -= OnSceneLoaded;

    }
}
