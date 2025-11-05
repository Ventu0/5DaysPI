using UnityEngine;
using System.Collections.Generic;
using System.IO;
public class NPCSaveData
{
    public string npcName;
    public bool alreadyRecievedQuest;
    public bool alreadyAnswered;
    public bool alreadyPayedMoney;
}
[System.Serializable]
public class NPCSaveDataList
{
    public List<NPCSaveData> npcSaveDatas = new List<NPCSaveData>();
}
public class SaveNPCs : MonoBehaviour
{
    [Header("Save Data")]
    [SerializeField] string jsonName;
    [SerializeField] string path;

    //public delegate void Salvar();
    //public Salvar onSave;
    [SerializeField] List<NPC> npcList;

    public static SaveNPCs instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            print("dois SaveNPCs na cena, autodestruição iminente");
            Destroy(gameObject);
        }
        path = Application.persistentDataPath + "/" + jsonName;
    }
    void Start()
    {
        
    }
    public void Save()
    {
        NPCSaveDataList saveDataList = new NPCSaveDataList();

        for(int i = 0; i < npcList.Count; i++)
        {
            NPCSaveData saveData = new NPCSaveData();
            NPC currentNPC = npcList[i];
            
        }


        //string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(path, json);
    }
    public void AddNPC(NPC npc)
    {
        if (!npcList.Contains(npc))
        {
            npcList.Add(npc);
        }
    }
}
