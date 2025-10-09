using UnityEngine;
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
        if (ChecarSePossuiSave())
        {
            string pasta = Application.persistentDataPath;
            string[] arquivos = Directory.GetFiles(pasta, "*.json");

            for (int i = 0; i < arquivos.Length; i++)
            {
                File.Delete(arquivos[i]); //se já houver um save, deleta ele
            }
        }
        PlayerPrefs.DeleteAll();
        if(Loader.instance != null)
            Loader.instance.DeletarTudoDoDontDestroy();
    }
    public bool ChecarSePossuiSave()
    {
        string caminho = Application.persistentDataPath;
        string[] arquivosJson = Directory.GetFiles(caminho, "*.json");
        return arquivosJson.Length > 0;
    }
}
