using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class Loader : MonoBehaviour
{
    public static Loader instance;
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
    public void DeletarTudo()
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
        CoisasParaSalvar coisasSalvas = new CoisasParaSalvar();
        if (File.Exists(caminho))
        {
            string json = File.ReadAllText(caminho);
            coisasSalvas = JsonUtility.FromJson<CoisasParaSalvar>(json);
        }

        SceneManager.LoadScene(coisasSalvas.activeScene);

        Player player = Player.instance;
        player.transform.position = coisasSalvas.playerPos;
        player.lastSavedPosition = coisasSalvas.playerLastSavedPos;
        Destroy(gameObject);
    }
}
