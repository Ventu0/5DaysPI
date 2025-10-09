using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Threading.Tasks;
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
            Destroy(objeto);
        }
        Destroy(gameObject);
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
    async void OnSceneLoaded(Scene scene, LoadSceneMode mode)
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

        money.AddMoneyNoAnimation(coisasSalvas.money);
        SceneManager.sceneLoaded -= OnSceneLoaded;
        await Task.Delay(100);
        Destroy(gameObject);
    }
}
