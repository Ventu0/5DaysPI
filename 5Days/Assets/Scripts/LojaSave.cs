using UnityEngine;
using System.Collections.Generic;
using System.IO;
public class LojaSaveInfo
{
    public List<bool> value = new List<bool>();
}
public class LojaSave : MonoBehaviour
{
    [SerializeField] string jsonName = "/LojaInfo.json";
    [SerializeField] string caminho;
    [SerializeField] LojaCharacterSelection characterSelection;
    PauseMenuController pauseMenu;
    private void Awake()
    {
        caminho = Application.persistentDataPath + jsonName;
    }
    void Start()
    {
        characterSelection = LojaCharacterSelection.instance;
        pauseMenu = PauseMenuController.instance;

        if (pauseMenu != null) pauseMenu.onSave += Save;
        Load();     

    }
    void Update()
    {
        
    }
    [ContextMenu("Salvar")]
    public void Save()
    {
        List<bool> unlocked = characterSelection.CheckWhichAreUnlocked();
        LojaSaveInfo save = new LojaSaveInfo();

        for(int i = 0; i < unlocked.Count; i++)
        {
            save.value.Add(unlocked[i]);
        }

        print(caminho);
        string json = JsonUtility.ToJson(save, true);
        File.WriteAllText(caminho, json);
    }
    public void Load()
    {
        if (!File.Exists(caminho))
        {
            characterSelection.Setup(null);
            return;
        }
        LojaSaveInfo save = new LojaSaveInfo();

        string json = File.ReadAllText(caminho);
        save = JsonUtility.FromJson<LojaSaveInfo>(json);

        characterSelection.Setup(save.value);
    }
    private void OnDisable()
    {
        if (pauseMenu != null) pauseMenu.onSave -= Save;
    }
}
