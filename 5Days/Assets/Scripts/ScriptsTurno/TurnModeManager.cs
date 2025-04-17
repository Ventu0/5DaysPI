using UnityEngine;
using System.Collections.Generic;
using System.Collections;
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
    [SerializeField] int qualInimigoVaiAtacar;
    public int turnoDeQualPersonagem;
    public Turnos turno;

    //variaveis invisiveis
    [SerializeField] List<BasePersonagem> aliadosPersonagens;
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
        }
        for(int i = 0; i < inimigos.Count; i++)
        {
            inimigosPersonagens.Add(inimigos[i].GetComponent<BasePersonagem>());
        }
    }
    void Start()
    {
        if (inimigos.Count > 1)
        {
            hasOnlyOneEnemy = false;
        }else if(inimigos.Count == 1)
        {
            hasOnlyOneEnemy = true;
        }
        InteractButtonsController.instance.SetupMenu(aliados[turnoDeQualPersonagem].transform.position);
        onPlayerTurn?.Invoke();
    }
    void Update()
    {
        //float vertical = Input.GetAxisRaw("Vertical");
        //if (!hasOnlyOneEnemy)
        //{
        //    StartCoroutine(MoverSeta(new Vector2();
        //}
        
    }
   
    IEnumerator MoverSeta(Vector2 newPos)
    {
        yield return null;
    }
    public void CheckIfAllPlayersAttacked()
    {
        bool todosAtacaram = false;
        if(turno == Turnos.PlayerTurn)
        {
            for (int i = 0; i < aliadosPersonagens.Count; i++)
            {
                if (aliadosPersonagens[i].jaAtacou == false)
                {
                    todosAtacaram = false;

                }
                else
                {
                    todosAtacaram = true;
                }
            }
            if (todosAtacaram)
            {
                print("turno do inimigo agorinha");
                InteractButtonsController.instance.menu.SetActive(false);
                turno = Turnos.EnemyTurn;
            }
            else if (!todosAtacaram)
            {
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
                {
                    todosAtacaram = false;

                }
                else
                {
                    todosAtacaram = true;
                }
            }
            if (todosAtacaram)
            {
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
        if (turno == Turnos.PlayerTurn)
        {
            if (!aliadosPersonagens[turnoDeQualPersonagem].jaAtacou)
            {
                return aliadosPersonagens[turnoDeQualPersonagem];
            }
            else
            {
                return null;
            }
        }
        else if (turno == Turnos.EnemyTurn)
        {
            if (!inimigosPersonagens[turnoDeQualPersonagem].jaAtacou)
            {
                return inimigosPersonagens[turnoDeQualPersonagem];
            }
            else
            {
                return null;
            }
        }
        else return null;   
    }
    public Animator PlayerAnimator()
    {
        return aliados[turnoDeQualPersonagem].GetComponent<Animator>();
    }
    public BasePersonagem EncontrarAlvo()
    {
        if (hasOnlyOneEnemy && turno == Turnos.PlayerTurn)
        {
            return inimigos[0].GetComponent<BasePersonagem>();
        }
        else
        {
            return null;
        }
    }
    #endregion
}
