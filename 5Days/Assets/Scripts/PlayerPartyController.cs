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
            partyAtual.Add(character);
        }
    }
    public void CurarTodos()
    {
        print("curando");
        for (int i = 0; i < playerParty.Count; i++)
        {
            playerParty[i].vidaAtual = playerParty[i].vidaMaxima;
        }
    }
}
