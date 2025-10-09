using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections;

public class Loader : MonoBehaviour
{
    CoisasParaSalvar coisasSalvas;
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
    public void DeletarTudoDoDontDestroy()
    {
        Scene dontDestroyScene = gameObject.scene;
        GameObject[] objectsInScene = dontDestroyScene.GetRootGameObjects();
        foreach(GameObject objeto in objectsInScene)
        {
            if(objeto != gameObject)
            Destroy(objeto);
        }
    }
    public void Carregar()
    {
        string caminho = Application.persistentDataPath + "/PlayerData.json";
         coisasSalvas = new CoisasParaSalvar();
        if (File.Exists(caminho))
        {
            string json = File.ReadAllText(caminho);
            coisasSalvas = JsonUtility.FromJson<CoisasParaSalvar>(json);
        }
        Destroy(DeleteSave.instance);
        SceneManager.LoadScene(coisasSalvas.activeScene);
    }
    IEnumerator DelayedAddMoney(PlayerMoney money, int amount)
    {
        yield return null; // espera 1 frame
        money.AddMoneyNoAnimation(amount);
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene == null || coisasSalvas == null) return;

        if(scene.name != coisasSalvas.activeScene)
            return;

        Player player = Player.instance;
        QuestController questController = QuestController.instance;
        PlayerMoney money = PlayerMoney.instance;

        if (player == null) print("Player nulo no Loader");
        player.transform.position = coisasSalvas.playerPos;
        player.lastSavedPosition = coisasSalvas.playerLastSavedPos;

        questController?.JustSetQuest(coisasSalvas.activeQuest);
        if (money == null) print("null");
        StartCoroutine(DelayedAddMoney(money, coisasSalvas.money));
        SceneManager.sceneLoaded -= OnSceneLoaded;

    }
}
