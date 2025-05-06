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
    public List<EnemyAI> inimigos;
    [Space]
    public List<Aliados> aliados;

    [Tooltip("uma UI de vitoria ou derrota")]
    [SerializeField] GameObject EndMenu;
    
    [Header("Debug")]
    [SerializeField] bool hasOnlyOneEnemy;
    public int turnoDeQualPersonagem;
    public Turnos turno;

    //variaveis invisiveis
    [HideInInspector] public List<BasePersonagem> aliadosPersonagens;
    [HideInInspector] public List<BasePersonagem> inimigosPersonagens;
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
        InteractButtonsController.instance.ataques = QuemEstaAtacando().GetComponent<Aliados>().ataques;
        InteractButtonsController.instance.SetupMenu(aliados[turnoDeQualPersonagem].transform.position);
        print("primeiro ataque do aliado");
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
        //float vertical = Input.GetAxisRaw("Vertical");
        //if (!hasOnlyOneEnemy)
        //{
        //    StartCoroutine(MoverSeta(new Vector2();
        //}

    }
    void EndGame()
    {
        EndMenu.SetActive(true);
        Time.timeScale = 0f;
    }
    IEnumerator MoverSeta(Vector2 newPos)
    {
        yield return null;
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
                inimigosPersonagens[turnoDeQualPersonagem].jaAtacou = false;
                for(int i = 0; i < aliadosPersonagens.Count; i++)
                {
                    aliadosPersonagens[i].jaAtacou = false;
                }
                InteractButtonsController.instance.SetupMenu(aliados[turnoDeQualPersonagem].transform.position);
                InteractButtonsController.instance.menu.SetActive(true);
            }
            else if(!JaAtacaram(inimigosPersonagens))
                turnoDeQualPersonagem += 1;
        }
    }
    #region Utils
    public bool JaAtacaram(List<BasePersonagem> personagems)
    {
        for(int i = 0; i < personagems.Count; i++)
        {
            if (personagems[i].jaAtacou == false)
                return false;
        }
        return true;
    }
    public BasePersonagem QuemEstaAtacando()
    {
        List<BasePersonagem> personagems = turno == Turnos.PlayerTurn ? aliadosPersonagens : turno == Turnos.EnemyTurn ? inimigosPersonagens : null;
        //verificador, se player turno for true, recebe aliadosPersonagens, se não, recebe inimigosPersonagens
        if (personagems == null)
            return null;

        return !personagems[turnoDeQualPersonagem].jaAtacou ? personagems[turnoDeQualPersonagem] : null;
    }
    public BasePersonagem EncontrarAlvo()
    {
        if (hasOnlyOneEnemy && turno == Turnos.PlayerTurn)
        {
            return inimigos[0].GetComponent<BasePersonagem>();
        }
        else if(turno == Turnos.EnemyTurn)
        {
            int aliadoEscolhido = Random.Range(0, aliados.Count);
            return aliadosPersonagens[aliadoEscolhido];
        }
        else
            return null;
    }
    #endregion
}
