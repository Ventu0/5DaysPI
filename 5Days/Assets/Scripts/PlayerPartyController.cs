using UnityEngine;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public class CharacterStatusData
{
    public string characterName;
    public int vidaAtual;
    public List<int> ppAtaques;
    public bool isDead;
}
[System.Serializable]
public class Party
{
    public List<CharacterStatusData> partyAtual = new List<CharacterStatusData>();
}
public class PlayerPartyController : MonoBehaviour
{
    [Header("Party Do Player")]
    [SerializeField] public List<CharacterStatusGeneric> playerParty = new List<CharacterStatusGeneric>();
    [Space]
    public List<CharacterStatusGeneric> partyAtual = new List<CharacterStatusGeneric>();
    Party party;
    public static PlayerPartyController instance;
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
        party = new Party();
    }
    void Start()
    {
        if(PauseMenuController.instance != null)
        PauseMenuController.instance.onSave += SaveParty;
        string caminho = Application.persistentDataPath + "/PlayerParty.json";
        if (File.Exists(caminho)) //se já houver uma party
        {
            LoadParty();
            return;
        }
        for(int i = 0; i < playerParty.Count; i++)
        {
            CharacterStatusGeneric character = Instantiate(playerParty[i]);
            List<Attack> ataques = character.ataques;
            character.name = playerParty[i].name;
            for (int j = 0; j < ataques.Count; j++)
            {
                if(ataques[j] != null)
                ataques[j].currentPP = ataques[j].maxPP;
            }
            partyAtual.Add(character);
        }
    }
    public void AddCharacter(CharacterStatusGeneric character)
    {
        if (playerParty.Count >= 3 || partyAtual.Count >= 3) return;
        playerParty.Add(character);
        partyAtual.Clear();

        partyAtual = new List<CharacterStatusGeneric>(playerParty);
    }
    #region SaveThings
    public void SaveParty()
    {
        List<CharacterStatusData> saveParty = party.partyAtual;
        saveParty.Clear();
        for(int i = 0; i < partyAtual.Count; i++)
        {
            saveParty.Add(ExtrairData(partyAtual[i])); //extrai as informações que quero salvar para Json poder ler
        }

        string json = JsonUtility.ToJson(party, true);
        File.WriteAllText(Application.persistentDataPath + "/PlayerParty.json", json);
        print("Salvando, Caminho: " + Application.persistentDataPath + "/PlayerParty.json");
    }
    CharacterStatusData ExtrairData(CharacterStatusGeneric original)
    {
        CharacterStatusData dados = new CharacterStatusData();
        dados.characterName = original.name;
        dados.vidaAtual = original.vidaAtual;
        dados.ppAtaques = new List<int>();
        dados.isDead = original.isDead;
        for (int i = 0; i < original.ataques.Count; i++)
        {
            if (original.ataques[i] != null)
                dados.ppAtaques.Add(original.ataques[i].currentPP);
        }
        return dados;
    }
    public void LoadParty()
    {
        string caminho = Application.persistentDataPath + "/PlayerParty.json";
        partyAtual.Clear();
        playerParty.Clear();
        string json = File.ReadAllText(caminho);
        Party savedParty = JsonUtility.FromJson<Party>(json); //usando variavel local pois não quero sobreescrever party, por mais que desse
        for(int i = 0; i < savedParty.partyAtual.Count; i++)
        {
            //pega EXATAMENTE o personagem da pasta Resources (por isso salvamos o nome original do personagem)
            CharacterStatusGeneric character = Resources.Load<CharacterStatusGeneric>("Characters/" + savedParty.partyAtual[i].characterName);
            CharacterStatusGeneric clone = Instantiate(character);
            clone.name = character.name;
            //modifica os valores de clone para serem os valores já salvos no Json(CharacterStatusData)
            clone.vidaAtual = savedParty.partyAtual[i].vidaAtual;
            clone.isDead = savedParty.partyAtual[i].isDead;
            for (int j = 0; j < clone.ataques.Count; j++)
            {
                if(clone.ataques[j] != null)
                clone.ataques[j].currentPP = savedParty.partyAtual[i].ppAtaques[j];
            }
            partyAtual.Add(clone);
        }
        playerParty = new List<CharacterStatusGeneric>(partyAtual);
    }
    #endregion
    public void CurarTodos()
    {
        print("curando");
        for (int i = 0; i < playerParty.Count; i++)
        {
            partyAtual[i].vidaAtual = playerParty[i].vidaMaxima;
            partyAtual[i].isDead = false;
            List<Attack> ataques = playerParty[i].ataques;
            for (int j = 0; j < ataques.Count; j++)
            {
                if(ataques[j])
                partyAtual[i].ataques[j].currentPP = ataques[j].maxPP;
            }
        }
    }
    private void OnDisable()
    {
        PauseMenuController pauseMenu = PauseMenuController.instance;
        if(pauseMenu != null)
        PauseMenuController.instance.onSave -= SaveParty;
    }
}
