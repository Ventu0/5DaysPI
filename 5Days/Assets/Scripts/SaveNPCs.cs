using UnityEngine;
using System.Collections.Generic;
using System.IO;
[System.Serializable]
public class NPCSaveData
{
    public string npcId;
    public bool alreadyRecievedQuest;
    public bool alreadyAnswered;
    public bool alreadyPayedMoney;
}
[System.Serializable]
public class NPCSaveDataList
{
    public List<NPCSaveData> npcData = new List<NPCSaveData>();
}
public class SaveNPCs : MonoBehaviour
{
    [Header("Save Data")]
    [SerializeField] string jsonName;
    [SerializeField] string path;

    //public delegate void Salvar();
    //public Salvar onSave;
    [SerializeField] List<NPC> npcList;
    List<NPCSaveData> dataFromBefore = new List<NPCSaveData>();
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
        Load();
    }
    void Start()
    {
        if(PauseMenuController.instance != null)
            PauseMenuController.instance.onSave += Save;
    }
    public void Save()
    {
        NPCSaveDataList saveDataList = new NPCSaveDataList();

        for(int i = 0; i < npcList.Count; i++)
        {
            NPCSaveData saveData = new NPCSaveData();
            NPC currentNPC = npcList[i];
            YesOrNo npcYesOrNo = currentNPC.yesOrNo;
            DialogueOptions options = npcYesOrNo?.options;

            if (options != null) 
            {
                if (options.needMoney) //se adicionar mais condições, adicionar aqui
                    saveData.alreadyPayedMoney = currentNPC.yesOrNo.conditionMet;
            } 
            saveData.npcId = currentNPC.idToSave;
            saveData.alreadyRecievedQuest = currentNPC.alreadyRecievedQuest;
            saveData.alreadyAnswered = currentNPC.alreadyTalked;
            saveDataList.npcData.Add(saveData);
        }

        string json = JsonUtility.ToJson(saveDataList, true);
        File.WriteAllText(path, json);
    }
    void Load()
    {
        if (HasData())
        {
            string json = File.ReadAllText(path);
            NPCSaveDataList saveDataList = JsonUtility.FromJson<NPCSaveDataList>(json);

            for(int i = 0; i < saveDataList.npcData.Count; i++)
            {
                dataFromBefore.Add(saveDataList.npcData[i]);
            }
        }
    }
    public bool HasData()
    {
        return File.Exists(path);
    }
    public NPCSaveData GetNPCData(string npcIds)
    {
        for (int i = 0; i < dataFromBefore.Count; i++)
        {
            if (dataFromBefore[i].npcId == npcIds)
            {
                return dataFromBefore[i];
            }
        }
        return null;
    }
    public void AddNPC(NPC npc)
    {
        if (!npcList.Contains(npc))
        {
            npcList.Add(npc);
        }
    }
    private void OnDisable()
    {
        if(PauseMenuController.instance != null)
        PauseMenuController.instance.onSave -= Save;
    }
}
