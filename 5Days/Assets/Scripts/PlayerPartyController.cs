using UnityEngine;
using System.Collections.Generic;
public class PlayerPartyController : MonoBehaviour
{
    [Header("Party Do Player")]
    [SerializeField] public List<CharacterStatusGeneric> playerParty = new List<CharacterStatusGeneric>();
    [Space]
    public List<CharacterStatusGeneric> partyAtual = new List<CharacterStatusGeneric>();
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
    }
    void Start()
    {
        for(int i = 0; i < playerParty.Count; i++)
        {
            CharacterStatusGeneric character = Instantiate(playerParty[i]);
            List<Attack> ataques = character.ataques;
            for (int j = 0; j < ataques.Count; j++)
            {
                if(ataques[j] != null)
                ataques[j].currentPP = ataques[j].maxPP;
            }
            partyAtual.Add(character);
        }
        CurarTodos();
    }
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
}
