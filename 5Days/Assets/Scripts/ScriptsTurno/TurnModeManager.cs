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
    public int turnoDeQualJogador;
    public Turnos turno;
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
        InteractButtonsController.instance.SetupMenu(aliados[turnoDeQualJogador].transform.position);
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
        for(int i = 0; i < aliados.Count; i++)
        {
            if (aliados[i].jaAtacou == false)
            {
                turnoDeQualJogador += 1;
                InteractButtonsController.instance.menu.SetActive(true);
                InteractButtonsController.instance.attackMenuAnim.gameObject.SetActive(false);
                InteractButtonsController.instance.SetupMenu(aliados[turnoDeQualJogador].transform.position);
                return;
            }
            else
            {
                print("turno do inimigo agorinha");
                InteractButtonsController.instance.menu.SetActive(false);
                turno = Turnos.EnemyTurn;
            }
        }
    }
    #region Utils
    public Aliados QuemEstaAtacando()
    {
        if (!aliados[turnoDeQualJogador].jaAtacou) return aliados[turnoDeQualJogador];
        else
            return null;
    }
    public Animator PlayerAnimator()
    {
        return aliados[turnoDeQualJogador].GetComponent<Animator>();
    }
    public BasePersonagem EncontrarAlvo()
    {
        if (hasOnlyOneEnemy)
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
