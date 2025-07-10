using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
public enum Turnos
{
    PlayerTurn,
    EnemyTurn
}

public class TurnModeManager : MonoBehaviour
{

    [Header("Essential")]
    public TextMeshProUGUI winText;
    public List<EnemyAI> inimigos;
    [Space]
    public List<Aliados> aliados;

    [SerializeField] GameObject EndMenu;
    [Tooltip("uma UI de vitoria ou derrota")]

    [Header("Debug")]
    [SerializeField] bool hasOnlyOneEnemy;
    [SerializeField] public Camera mainCamera;
    public float escapeChance;
    [Tooltip("Chance de escapar da batalha, é usado em porcentagem, ou seja, o numero é entre 0 a 1")]
    public int turnoDeQualPersonagem;
    public Turnos turno;

    //variaveis invisiveis
    public List<BasePersonagem> aliadosPersonagens;
    public List<BasePersonagem> aliadosPersonagensPersistentes; //não é usado no sistema, somente no final
    public List<BasePersonagem> inimigosPersonagens;
    public IniciarLuta iniciarLuta;
    public static TurnModeManager instance;
    private void Awake()
    {
        turno = Turnos.PlayerTurn;
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        EndMenu.gameObject.SetActive(false);

        turnoDeQualPersonagem = 0;
    }
    public void FirstAllyAttack()
    {
        InteractButtonsController.instance.ataques = QuemEstaAtacando().characterStatus.ataques;
        InteractButtonsController.instance.SetupMenu(aliados[turnoDeQualPersonagem].transform.position);
    }
    void Update()
    {
        if (inimigos.Count > 1)
        {
            hasOnlyOneEnemy = false;
        }
        else if (inimigos.Count == 1)
        {
            hasOnlyOneEnemy = true;
        }
    }
    void EndGame()
    {
        EndMenu.SetActive(true);
        #region ManterStatusAposALuta
        PlayerPartyController party = PlayerPartyController.instance;
        //int aliadosMortos =     terminar isso aqui    
        for(int i = 0; i < aliadosPersonagens.Count; i++)
        {
            BasePersonagem personagem = aliadosPersonagens[i];
            BasePersonagem personagemPersistente = aliadosPersonagensPersistentes[i];
            party.partyAtual[i].vidaAtual = personagem.vidaAtual;
            if (party.partyAtual[i].isDead)
            party.partyAtual[i].isDead = personagemPersistente.characterStatus.isDead;
            for (int j = 0; j < personagem.characterStatus.ataques.Count; j++)
            {
                Attack ataque = party.partyAtual[i].ataques[j];
                if (personagem.characterStatus.ataques[j] != null)
                {
                    ataque.oneTime = false;
                    ataque.currentPP = personagem.characterStatus.ataques[j].currentPP; 
                }
            }
        }
        #endregion
        iniciarLuta.EndBattle();
        iniciarLuta = null;
    }
    public void CheckIfAllCharactersAttacked()
    {
        if(turno == Turnos.PlayerTurn)
        {
            if (inimigos.Count <= 0)
            {
                EndGame();
                return;
            }
            if (JaAtacaram(aliadosPersonagens))
            {
                turno = Turnos.EnemyTurn;
                InteractButtonsController.instance.menu.SetActive(false);
                turnoDeQualPersonagem = 0;
                inimigos[turnoDeQualPersonagem].Attack();
            }
            else if (!JaAtacaram(aliadosPersonagens))
            {
                turnoDeQualPersonagem += 1;
                InteractButtonsController.instance.NextPlayer();
            }
        }
        else if(turno == Turnos.EnemyTurn)
        {
            if (aliados.Count <= 0)
            {
                EndGame();
                return;
            }
            turnoDeQualPersonagem = 0;
             
            if (JaAtacaram(inimigosPersonagens))
            {
                turno = Turnos.PlayerTurn;
                for(int i = 0; i < inimigosPersonagens.Count; i++)
                {
                    inimigosPersonagens[i].turnEnded = false;
                }
                for (int i = 0; i < aliadosPersonagens.Count; i++)
                {
                    aliados[i].shield.SetActive(false);
                    aliados[i].isDefending = false;
                    aliadosPersonagens[i].turnEnded = false;
                    aliadosPersonagens[i].OnTurnStart();
                }
                InteractButtonsController interactButtonsController = InteractButtonsController.instance;
                interactButtonsController.runButton.enabled = true;
                interactButtonsController.runButton.gameObject.SetActive(true);
                interactButtonsController.NextPlayer();
                interactButtonsController.menu.SetActive(true);
                interactButtonsController.attackMenuAnim.gameObject.SetActive(false);
            }
            else if (!JaAtacaram(inimigosPersonagens))
            {
                turnoDeQualPersonagem += 1;
                inimigos[turnoDeQualPersonagem].Attack();
            }
        }
    }
    #region Utils
    public bool JaAtacaram(List<BasePersonagem> personagems)
    {
        for(int i = 0; i < personagems.Count; i++)
        {
            if (personagems[i].turnEnded == false)
                return false;
        }
        return true;
    }
    public BasePersonagem QuemEstaAtacando()
    {
        List<BasePersonagem> personagems = turno == Turnos.PlayerTurn ? aliadosPersonagens : turno == Turnos.EnemyTurn ? inimigosPersonagens : null;
        //verificador, se player turno for true, recebe aliadosPersonagens, se não, recebe inimigosPersonagens

        BasePersonagem personagem = personagems[turnoDeQualPersonagem];

        return personagem;
    }
    public BasePersonagem EncontrarAlvo()
    {
        if (hasOnlyOneEnemy && turno == Turnos.PlayerTurn)
        {
            return inimigos[0].GetComponent<BasePersonagem>();
        }
        else if (turno == Turnos.EnemyTurn)
        {
            int aliadoEscolhido = Random.Range(0, aliados.Count);
            return aliadosPersonagens[aliadoEscolhido];
        }
        else
            return null;
    }
    #endregion
}
