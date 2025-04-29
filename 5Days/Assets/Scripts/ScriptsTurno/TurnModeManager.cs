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
    public delegate void PlayerTurn();
    public PlayerTurn onPlayerTurn;
    public static TurnModeManager instance;
    public List<EnemyAI> inimigos;
    [SerializeField] bool hasOnlyOneEnemy;
    public List<Aliados> aliados;
    [SerializeField] GameObject decisionMenu;
    [SerializeField] int qualInimigoVaiAtacar;
    public int turnoDeQualPersonagem;
    public Turnos turno;

    //variaveis invisiveis
    public List<BasePersonagem> aliadosPersonagens;
    public List<BasePersonagem> inimigosPersonagens;
    private void Awake()
    {
        turno = Turnos.PlayerTurn;
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        for(int i = 0; i < aliados.Count; i++)
        {
            aliadosPersonagens.Add(aliados[i].GetComponent<BasePersonagem>());
            aliadosPersonagens[i].numeroDoPersonagem = i;
        }
        turnoDeQualPersonagem = 0;

    }
    void Start()
    {
        decisionMenu.gameObject.SetActive(false);
        
        InteractButtonsController.instance.SetupMenu(aliados[turnoDeQualPersonagem].transform.position);
        onPlayerTurn?.Invoke(); //depois do protótipo, arrumar o codigo inteiro para deixar organizado (inclui o delegate da linha).
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
    void EndTurn()
    {
        decisionMenu.SetActive(true);
        Time.timeScale = 0f;
    }
    IEnumerator MoverSeta(Vector2 newPos)
    {
        yield return null;
    }
    public void CheckIfAllPlayersAttacked()
    {
        if(turno == Turnos.PlayerTurn)
        {
            if (JaAtacaram(aliadosPersonagens))
            {
                InteractButtonsController.instance.menu.SetActive(false);
                turno = Turnos.EnemyTurn;
                turnoDeQualPersonagem = 0;
                inimigos[turnoDeQualPersonagem].Attack();
            }
            else if (!JaAtacaram(aliadosPersonagens))
            {
                if (inimigos.Count <= 0)
                {
                    EndTurn();
                    return;
                }

                turnoDeQualPersonagem += 1;
                InteractButtonsController.instance.NextPlayer();
            }
        }
        else if(turno == Turnos.EnemyTurn)
        {
            turnoDeQualPersonagem = 0;
             
            if (JaAtacaram(inimigosPersonagens))
            {
                if (aliados.Count <= 0)
                {
                    EndTurn();
                    return;
                }
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
            {
                turnoDeQualPersonagem += 1;
            }
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
        {
            return null;
        }
    }
    #endregion
}
