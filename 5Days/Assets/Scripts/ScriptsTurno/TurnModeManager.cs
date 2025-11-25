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
    [SerializeField] TextMeshProUGUI endText;
    [Tooltip("uma UI de vitoria ou derrota")]
    public Camera turnModeCam;
    [SerializeField] BackGroundControl backGroundControl;

    [Header("Debug")]
    [SerializeField] bool hasOnlyOneEnemy;
    [Tooltip("Chance de escapar da batalha, é usado em porcentagem, ou seja, o numero é entre 0 a 1")]
    public float escapeChance;
    public int turnoDeQualPersonagem;
    public Turnos turno;

    //variaveis invisiveis
    [HideInInspector] public List<BasePersonagem> aliadosPersonagens;
    [HideInInspector] public List<BasePersonagem> aliadosPersonagensPersistentes; //não é usado no sistema, somente no final
    [HideInInspector] public List<BasePersonagem> inimigosPersonagens;
    [HideInInspector] public Vector3 originalCameraPos;
    
    public EnemyIniciarLuta currentFightingEnemy;
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
        originalCameraPos = turnModeCam.transform.position;
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
    [ContextMenu("Perder")]
    void FinalizarAgora()
    {
        EndGame(false);
    }
    public void EndGame(bool winOrLose = false)
    {
        EndMenu.SetActive(true);
        if (winOrLose)
        {
            endText.text = "Vitória!!!";
            endText.color = Color.green;
            currentFightingEnemy.EndBattle();
        }
        else
        {
            endText.text = "Derrota!!!";
            endText.color = Color.red;
            ReturnScene.instance.AddLoseMethod();
        }
           
        MaintainStatus();
    }
    public void MaintainStatus()
    {
        #region ManterStatusAposALuta
        PlayerPartyController party = PlayerPartyController.instance;

        for (int i = 0; i < aliadosPersonagens.Count; i++)
        {
            BasePersonagem personagem = aliadosPersonagens[i];
            BasePersonagem personagemPersistente = aliadosPersonagensPersistentes[i];
            party.partyAtual[i].vidaAtual = personagemPersistente.vidaAtual;

            if (party.partyAtual[i].isDead) print("eu to morto: " + personagem.characterStatus.name);

            party.partyAtual[i].isDead = personagemPersistente.characterStatus.isDead;
            //aqui tem chance de dar erro(linha de cima). porque? por conta que eu acho que personagemPersistente sempre retornaria true ou sempre false, o que pode dar problema
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
    }

    public void CheckIfAllCharactersAttacked()
    {
        if(turno == Turnos.PlayerTurn)
        {
            if(inimigos.Count <= 0)
            {
                EndGame(true);
                return;
            }
            if (JaAtacaram(aliadosPersonagens))
            {
                turno = Turnos.EnemyTurn;
                InteractButtonsController.instance.menu.SetActive(false);
                for(int i = 0; i < inimigosPersonagens.Count; i++)
                {
                    inimigosPersonagens[i].OnTurnStart();
                }
                turnoDeQualPersonagem = 0;
                if (inimigos.Count <= 0)
                {
                    EndGame(true);
                    return;
                }
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
                EndGame(false);
                return;
            }
             
            if (JaAtacaram(inimigosPersonagens))
            {
                turnoDeQualPersonagem = 0;
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
                if (aliados.Count <= 0)
                {
                    EndGame(false);
                    return;
                }
                interactButtonsController.NextPlayer();

                interactButtonsController.menu.SetActive(true);
                interactButtonsController.attackMenuAnim.gameObject.SetActive(false);
            }
            else if (!JaAtacaram(inimigosPersonagens))
            {
                print("nao acabou o turno: " + turnoDeQualPersonagem);
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
            {
                return false;
            }
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