using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Inimigos
{
    public Inimigo[] enemies; //fiz a classe para ser mais facil para converter em Json
}
public class InimigosController : MonoBehaviour
{
    [SerializeField] IniciarLuta[] inimigosArray;
    public static InimigosController instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        PauseMenuController.instance.onSave += Salvar;
    }
    void Start()
    {
        inimigosArray = GetComponentsInChildren<IniciarLuta>();   
        for(int i = 0; i < inimigosArray.Length; i++)
        {
            inimigosArray[i].personalID = i + 1; //da o ID, evitando 0,1,2 para ficar 1,2,3
        }
        Carregar();
    }
    public void Salvar()
    {
        Inimigos inimigos = new Inimigos();
        inimigos.enemies = new Inimigo[inimigosArray.Length];

        for (int i = 0; i < inimigosArray.Length; i++)
        {
            bool morreu = inimigosArray[i].inimigo.jaMorreu;
            inimigos.enemies[i] = new Inimigo(morreu);
        }
        string json = JsonUtility.ToJson(inimigos, true);
        File.WriteAllText(Application.persistentDataPath + "/inimigos.json", json);
    }
    public void Carregar()
    {
        string caminho = Application.persistentDataPath + "/inimigos.json";
        print(caminho);
        Inimigos inimigosSalvos = new Inimigos();
        if (File.Exists(caminho))
        {
            string json = File.ReadAllText(caminho);
            inimigosSalvos = JsonUtility.FromJson<Inimigos>(json);
        }
        else return;

        for(int i = 0; i < inimigosSalvos.enemies.Length; i++)
        {
            if (inimigosSalvos.enemies[i].jaMorreu)
            {
                inimigosArray[i].inimigo.jaMorreu = inimigosSalvos.enemies[i].jaMorreu;
                inimigosArray[i].gameObject.SetActive(false);
            }
        }
    }
    private void OnDisable()
    {
        PauseMenuController.instance.onSave -= Salvar;
    }
}
