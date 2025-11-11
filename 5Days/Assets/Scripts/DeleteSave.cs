using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections.Generic;
public class DeleteSave : MonoBehaviour
{
    public static DeleteSave instance;
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
    void Update()
    {
        
    }
    [ContextMenu("Deletar Save")]
    public void Deletar()
    {
        PlayerPrefs.DeleteAll();
        if (ChecarSePossuiSave())
        {
            print("apagando save");

            string pasta = Application.persistentDataPath;
            string[] arquivos = Directory.GetFiles(pasta, "*.json");

            for (int i = 0; i < arquivos.Length; i++)
            {
                File.Delete(arquivos[i]); //se já houver um save, deleta ele
            }
        }
        DeletarTudoDoDontDestroy();
    }
    public void DeletarTudoDoDontDestroy()
    {
        Scene dontDestroyScene = gameObject.scene;
        GameObject[] objectsInScene = dontDestroyScene.GetRootGameObjects();
        foreach (GameObject objeto in objectsInScene)
        {
            if (objeto != gameObject)
                Destroy(objeto);
        }
    }
    public bool ChecarSePossuiSave()
    {
        print("checando save");
        string caminho = Application.persistentDataPath;
        string[] arquivosJson = Directory.GetFiles(caminho, "*.json");
        bool check = arquivosJson.Length > 0;
         print("chequei e o resultado deu: " + check);
        return check;
    }
}
