using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;
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
    [SerializeField] List<EnemyAI> inimigos;
    [SerializeField] bool hasOnlyOneEnemy;
    public List<Aliados> aliados;
    [SerializeField] TextMeshProUGUI winText;
    [SerializeField] int qualInimigoVaiAtacar;
    public int turnoDeQualPersonagem;
    public Turnos turno;

    //variaveis invisiveis
    public List<BasePersonagem> aliadosPersonagens;
    [SerializeField] List<BasePersonagem> inimigosPersonagens;
    private void Awake()
    {
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
        for(int i = 0; i < inimigos.Count; i++)
        {
            inimigosPersonagens.Add(inimigos[i].GetComponent<BasePersonagem>());
        }
        turnoDeQualPersonagem = 0;
    }
    void Start()
    {
        winText.gameObject.SetActive(false);
        if (inimigos.Count > 1)
        {
            hasOnlyOneEnemy = false;
        }else if(inimigos.Count == 1)
        {
            hasOnlyOneEnemy = true;
        }
        InteractButtonsController.instance.SetupMenu(aliados[turnoDeQualPersonagem].transform.position);
        onPlayerTurn?.Invoke(); //depois do protótipo, arrumar o codigo inteiro para deixar organizado (inclui o delegate da linha).
    }
    void Update()
    {
        //float vertical = Input.GetAxisRaw("Vertical");
        //if (!hasOnlyOneEnemy)
        //{
        //    StartCoroutine(MoverSeta(new Vector2();
        //}
        
    }
    void Vitoria()
    {
        winText.gameObject.SetActive(true);
    }
    IEnumerator MoverSeta(Vector2 newPos)
    {
        yield return null;
    }
    public void CheckIfAllPlayersAttacked()
    {
        bool todosAtacaram = false;
        //inimigos[turnoDeQualPersonagem].canAttack = true;
        if(turno == Turnos.PlayerTurn)
        {
            for (int i = 0; i < aliadosPersonagens.Count; i++)
            {
                if (aliadosPersonagens[i].jaAtacou == false)
                    todosAtacaram = false;
                else
                {
                    todosAtacaram = true;
                    aliadosPersonagens[i].jaAtacou = false;
                }
            }
            if (todosAtacaram)
            {
                print("turno do inimigo agorinha");
                InteractButtonsController.instance.menu.SetActive(false);
                turno = Turnos.EnemyTurn;
                turnoDeQualPersonagem = 0;
                inimigos[turnoDeQualPersonagem].Attack();
            }
            else if (!todosAtacaram)
            {
                if (inimigos.Count <= 0)
                {
                    Vitoria();
                }
                
                turnoDeQualPersonagem += 1;
                InteractButtonsController.instance.NextPlayer();
                return;
            }
        }
        else if(turno == Turnos.EnemyTurn)
        {
            turnoDeQualPersonagem = 0;
            for (int i = 0; i < inimigosPersonagens.Count; i++)
            {
                if (inimigosPersonagens[i].jaAtacou == false)
                    todosAtacaram = false;
                else
                    todosAtacaram = true;
            }
            if (todosAtacaram)
            {
                turno = Turnos.PlayerTurn;
                print("mudando para turno: " + turno);
                inimigos[turnoDeQualPersonagem].canAttack = true;
                inimigosPersonagens[turnoDeQualPersonagem].jaAtacou = false;
                InteractButtonsController.instance.SetupMenu(aliados[turnoDeQualPersonagem].transform.position);
                InteractButtonsController.instance.menu.SetActive(true);
            }
            else if(!todosAtacaram)
            {
                turnoDeQualPersonagem += 1;
            }
        }
    }
    #region Utils
    public BasePersonagem QuemEstaAtacando()
    {
        print("atacando: " + turnoDeQualPersonagem);
        if (turno == Turnos.PlayerTurn)
        {
            if (!aliadosPersonagens[turnoDeQualPersonagem].jaAtacou)
            {
                return aliadosPersonagens[turnoDeQualPersonagem];
            }
            else return null;
        }
        else if (turno == Turnos.EnemyTurn)
        {
            if (!inimigosPersonagens[turnoDeQualPersonagem].jaAtacou)
            {
                return inimigosPersonagens[turnoDeQualPersonagem];
            }
            else return null;
        }
        else return null;   
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
