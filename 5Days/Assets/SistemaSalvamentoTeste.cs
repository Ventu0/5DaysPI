using UnityEngine;
using System.IO;
public class Tools
{
    public int dinheiro;
}
public class SistemaSalvamentoTeste : MonoBehaviour
{
    [SerializeField] int teste;
    [SerializeField] string path;
    void Start()
    {
        path = Application.persistentDataPath + "/sistemaSalvamentoTeste.json";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Save(SistemaSalvamentoTeste data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
    }
    public SistemaSalvamentoTeste Load()
    {
        if (!File.Exists(path))
        {
            return null;
        }
        string json = File.ReadAllText(path);
        SistemaSalvamentoTeste tools = JsonUtility.FromJson<SistemaSalvamentoTeste>(json);
        return tools;
    }
}
